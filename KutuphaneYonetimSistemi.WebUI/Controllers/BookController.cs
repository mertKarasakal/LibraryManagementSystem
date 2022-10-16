using System;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AForge.Imaging.Filters;
using IronOcr;
using LibraryManagementSystem.WebUI.Models;
using LibraryManagementSystem.WebUI.Models.EntityFramework;
using LibraryManagementSystem.WebUI.Security;

namespace LibraryManagementSystem.WebUI.Controllers
{
    [Authorize(Roles = "1,2")]
    public class BookController : Controller
    {
        private LibraryContext db = new LibraryContext();

        public ActionResult Index(string name, string isbn)
        {
            var books = from item in db.Books select item;
            if (!string.IsNullOrEmpty(name))
                books = books.Where(m => m.Name.Contains(name));
            if (!string.IsNullOrEmpty(isbn))
                books = books.Where(m => m.Isbn.Contains(isbn));
            return View(books.ToList());
        }

        public ActionResult BooksOnUser()
        {
            var book = db.Books.Include(k => k.Category).Include(k => k.User).Where(x => x.UserId == PersonelRoleProvider.personelId1);
            return View(book);
        }

        public ActionResult List(int? id)
        {
            var book = db.Books.Include(k => k.Category).Include(k => k.User).Where(x => x.CategoryId == id).ToList();
            return PartialView(book);
        }

        public ActionResult KullanicidakiKitaplar(int? id)
        {
            var book = db.Books.Include(k => k.Category).Include(k => k.User).Where(x => x.CategoryId == id).ToList();
            return PartialView(book);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        public string GoruntuIsleme(HttpPostedFileBase uploadFile)
        {
            Bitmap bmpPostedImage = new System.Drawing.Bitmap(uploadFile.InputStream);
            Bitmap newImage = new Bitmap(bmpPostedImage.Width, bmpPostedImage.Height);
            for (int i = 0; i < bmpPostedImage.Width; i++)
            {
                for (int j = 0; j < bmpPostedImage.Height; j++)
                {
                    Color color = bmpPostedImage.GetPixel(i, j);
                    int newColor = (color.R + color.G + color.B) / 3;
                    newImage.SetPixel(i, j, Color.FromArgb(newColor, newColor, newColor));
                }
            }
            bmpPostedImage = Sharpening(newImage);
            AutoOcr OCR = new AutoOcr() { ReadBarCodes = false };
            var results = OCR.Read(bmpPostedImage);
            string words = results.ToString();
            int index = words.IndexOf("ISBN") + 5;
            words = words.Substring(index, words.Length - index);
            index = words.IndexOf("\n");
            words = words.Substring(0, index - 1);
            for (int i = 0; i < words.Length; i++)
            {
                int number = 0;
                for (int j = 0; j < 10; j++)
                {
                    string ch = j.ToString();
                    if (words[i].Equals(ch[0]))
                    {
                        number = 1;
                        break;
                    }
                }
                if (number == 0)
                {
                    words = words.Remove(i, 1);
                    i--;
                }
            }
            return words;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,CategoryId,Name,Isbn,Author,Description,NumberOfPages")] Book book, HttpPostedFileBase uploadFile)
        {
            string words = GoruntuIsleme(uploadFile);
            book.Isbn = words;
            ViewBag.ISBNN = words;
            book.DeliveryTime = PersonelRoleProvider.MevcutGun.AddDays(7);
            book.Status = "BOSTA";//todo::? hardcoded mı?
            if (ModelState.IsValid)
            {
                if (uploadFile != null)
                {
                    uploadFile.SaveAs(HttpContext.Server.MapPath("~/Images/")
                                                          + uploadFile.FileName);
                    book.ImagePath = uploadFile.FileName;
                }
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", book.CategoryId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", book.UserId);
            return View(book);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Book book = db.Books.Find(id);
            if (book == null)
                return HttpNotFound();
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", book.CategoryId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", book.UserId);
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,CategoryId,Name,Isbn,Author,Description,NumberOfPages,ImagePath")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", book.CategoryId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", book.UserId);
            return View(book);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Book book = db.Books.Find(id);
            if (book == null)
                return HttpNotFound();
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult OduncKitapAl()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OduncKitapAl([Bind(Include = "Isbn")] BookModel kitapModel, HttpPostedFileBase uploadFile)
        {
            string isbn = GoruntuIsleme(uploadFile);
            if (ModelState.IsValid)
            {
                if (uploadFile != null)
                {
                    uploadFile.SaveAs(HttpContext.Server.MapPath("~/Images/") + uploadFile.FileName);
                    kitapModel.ImagePath = uploadFile.FileName;
                }
                var kitapEsit = db.Books.FirstOrDefault(m => m.Isbn == isbn);
                int kitapSayi = db.Books.Where(m => m.UserId == PersonelRoleProvider.personelId1).Count();
                if (kitapEsit != null)
                {
                    var linq = from p in db.Books
                               where p.Status == "teslimtarihigecmis" && p.UserId == PersonelRoleProvider.personelId1
                               select p;
                    int say = linq.Count();
                    if (say > 0)
                        ViewBag.gecmis = "üzerinizde teslim tarihi gecmis kitaplar var";
                    else
                    {
                        if (kitapSayi >= 3)
                            ViewBag.Olumsuz2 = "Üzerinizde zaten 3 kitap olduğu için başka kitap alamazsınız.";
                        else if (db.Books.FirstOrDefault(m => m.Isbn == isbn).Status != "BOSTA")
                            ViewBag.Olumsuz2 = "Bu kitap başka bir kullanıcının üzerinde!";
                        else
                        {
                            ViewBag.Olumlu = "Kitabı alma işlemi başarı ile gerçekleştirildi.";
                            db.Books.FirstOrDefault(m => m.Isbn == isbn).UserId = PersonelRoleProvider.personelId1;
                            db.Books.FirstOrDefault(m => m.Isbn == isbn).DeliveryTime = DateTime.Now.AddDays(7);
                            db.Books.FirstOrDefault(m => m.Isbn == isbn).Status = "KULLANICIDA";
                            db.SaveChanges();
                        }
                    }
                }
                else
                    ViewBag.Olumsuz = "Böyle bir kitap mevcut değil!";
                return View(kitapModel);
            }
            return View(kitapModel);
        }

        public ActionResult KitapTeslimEt()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KitapTeslimEt([Bind(Include = "Isbn")] BookModel kitapModel, HttpPostedFileBase uploadFile)
        {
            string isbn = GoruntuIsleme(uploadFile);
            if (ModelState.IsValid)
            {
                if (uploadFile != null)
                {
                    uploadFile.SaveAs(HttpContext.Server.MapPath("~/Images/") + uploadFile.FileName);
                    kitapModel.ImagePath = uploadFile.FileName;
                }
                var kitapEsit = db.Books.FirstOrDefault(m => m.Isbn == isbn);
                if (kitapEsit != null && kitapEsit.UserId == PersonelRoleProvider.personelId1)
                {
                    ViewBag.Olumlu = "Kitabı geri verme işlemi başarı ile gerçekleştirildi.";
                    db.Books.FirstOrDefault(m => m.Isbn == isbn).UserId = 1;
                    db.Books.FirstOrDefault(m => m.Isbn == isbn).Status = "BOSTA";
                    db.SaveChanges();
                }
                else
                    ViewBag.Olumsuz = "Böyle bir kitap yok veya size tanımlı değil!";
                return View(kitapModel);
            }
            return View(kitapModel);
        }

        public Bitmap Sharpening(Bitmap image)
        {
            Bitmap blurred = MedianFilter(image);
            Bitmap edge = EdgeDetection(image, blurred);
            image = SharpeImage(image, edge);
            return image;
        }

        public Bitmap MedianFilter(Bitmap image)
        {
            Bitmap imageX = new Bitmap(image);
            Bitmap bp = AForge.Imaging.Image.Clone(imageX, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Median filter = new Median();
            Bitmap newImage = filter.Apply(bp);
            return newImage;
        }

        public Bitmap SharpeImage(Bitmap OriginalImage, Bitmap Edge)
        {
            Color color1, color2, color;
            Bitmap Image;
            int width = OriginalImage.Width;
            int height = OriginalImage.Height;
            Image = new Bitmap(width, height);
            int R, G, B;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    color1 = OriginalImage.GetPixel(x, y);
                    color2 = Edge.GetPixel(x, y);
                    R = color1.R + color2.R; G = color1.G + color2.G; B = color1.B + color2.B;
                    if (R > 255) R = 255;
                    if (G > 255) G = 255;
                    if (B > 255) B = 255;
                    if (R < 0) R = 0;
                    if (G < 0) G = 0;
                    if (B < 0) B = 0;
                    color = Color.FromArgb(R, G, B);
                    Image.SetPixel(x, y, color);
                }
            }
            return Image;
        }

        public Bitmap EdgeDetection(Bitmap OriginalImage, Bitmap BlurredImage)
        {
            Color color1, color2, color;
            Bitmap Image;
            int width = OriginalImage.Width;
            int height = OriginalImage.Height;
            Image = new Bitmap(width, height);
            int R, G, B;
            double db = 1.7;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    color1 = OriginalImage.GetPixel(x, y);
                    color2 = BlurredImage.GetPixel(x, y);
                    R = Convert.ToInt16(db * (color1.R - color2.R));
                    G = Convert.ToInt16(db * (color1.G - color2.G));
                    B = Convert.ToInt16(db * (color1.B - color2.B));
                    if (R > 255) R = 255;
                    if (G > 255) G = 255;
                    if (B > 255) B = 255;
                    if (R < 0) R = 0;
                    if (G < 0) G = 0;
                    if (B < 0) B = 0;
                    color = Color.FromArgb(R, G, B);
                    Image.SetPixel(x, y, color);
                }
            }
            return Image;
        }

    }
}
