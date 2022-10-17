using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryManagementSystem.WebUI.Business.Abstract;
using LibraryManagementSystem.WebUI.Business.Concrete;
using LibraryManagementSystem.WebUI.Entity.Concrete;
using LibraryManagementSystem.WebUI.Entity.Concrete.Models;
using LibraryManagementSystem.WebUI.Security;
using LibraryManagementSystem.WebUI.Utilities.Constants;

namespace LibraryManagementSystem.WebUI.Controllers {
    [Authorize(Roles = "1,2")]
    public class BookController : Controller {
        private readonly IBookService _bookManager;
        private readonly IImageProcessingService _imageProcessing;
        private readonly ICategoryService _categoryManager;
        private readonly IUserService _userManager;

        public BookController() {
            _bookManager = new BookManager();
            _categoryManager = new CategoryManager();
            _userManager = new UserManager();
            _imageProcessing = new ImageProcessingManager();
        }

        public ActionResult Index(string name, string isbn) {
            //var books = from item in db.Books select item;
            var books = _bookManager.GetList().Data;
            if (!string.IsNullOrEmpty(name))
                books = books.Where(m => m.Name.Contains(name)).ToList();
            if (!string.IsNullOrEmpty(isbn))
                books = books.Where(m => m.Isbn.Contains(isbn)).ToList();
            return View(books.ToList());
        }

        public ActionResult BooksOnUser() {
            //todo::remove::var book = db.Books.Include(k => k.Category).Include(k => k.User).Where(x => x.UserId == PersonelRoleProvider.personelId1);
            return View(_bookManager.GetListByUser(PersonelRoleProvider.personelId1).Data);
        }
        public ActionResult KullanicidakiKitaplar(int id) {
            //var book = db.Books.Include(k => k.Category).Include(k => k.User).Where(x => x.CategoryId == id).ToList();
            return PartialView(_bookManager.GetListByCategory(id).Data);
        }

        public ActionResult List(int? id) {
            //todo::remove::var book = db.Books.Include(k => k.Category).Include(k => k.User).Where(x => x.CategoryId == id).ToList();
            return PartialView(_bookManager.GetList().Data);
        }



        [HttpGet]
        public ActionResult Create() {
            ViewBag.CategoryId = new SelectList(_categoryManager.GetList().Data, "Id", "Name");
            ViewBag.UserId = new SelectList(_userManager.GetList().Data, "Id", "Name");
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,CategoryId,Name,Isbn,Author,Description,NumberOfPages")] Book book, HttpPostedFileBase uploadFile) {
            //string words = GoruntuIsleme(uploadFile);
            string words = _imageProcessing.ProcessImage(uploadFile).Data;
            book.Isbn = words;
            ViewBag.ISBNN = words;
            book.DeliveryTime = PersonelRoleProvider.MevcutGun.AddDays(7);
            book.Status = BookStatus.Available;
            if (ModelState.IsValid) {
                if (uploadFile != null) {
                    uploadFile.SaveAs(HttpContext.Server.MapPath("~/Images/")
                                                          + uploadFile.FileName);
                    book.ImagePath = uploadFile.FileName;
                }
                //db.Books.Add(book);
                //db.SaveChanges();
                _bookManager.Add(book);
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(_categoryManager.GetList().Data, "Id", "Name", book.CategoryId);
            ViewBag.UserId = new SelectList(_userManager.GetList().Data, "Id", "Name", book.UserId);
            return View(book);
        }

        public ActionResult Edit(int id) {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //Book book = db.Books.Find(id);
            Book book = _bookManager.GetById(id).Data;
            if (book == null)
                return HttpNotFound();
            ViewBag.CategoryId = new SelectList(_categoryManager.GetList().Data, "Id", "Name", book.CategoryId);
            ViewBag.UserId = new SelectList(_userManager.GetList().Data, "Id", "Name", book.UserId);
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,CategoryId,Name,Isbn,Author,Description,NumberOfPages,ImagePath")] Book book) {
            if (ModelState.IsValid) {
                //db.Entry(book).State = EntityState.Modified;
                //db.SaveChanges();
                _bookManager.Update(book);
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(_categoryManager.GetList().Data, "Id", "Name", book.CategoryId);
            ViewBag.UserId = new SelectList(_userManager.GetList().Data, "Id", "Name", book.UserId);
            return View(book);
        }

        public ActionResult Delete(int id) {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //Book book = db.Books.Find(id);
            Book book = _bookManager.GetById(id).Data;
            if (book == null)
                return HttpNotFound();
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            //Book book = db.Books.Find(id);
            Book book = _bookManager.GetById(id).Data;
            //db.Books.Remove(book);
            //db.SaveChanges();
            _bookManager.Delete(book);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing) {
            if (disposing)
                //db.Dispose();
                base.Dispose(disposing);
        }

        public ActionResult BorrowBook() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BorrowBook([Bind(Include = "Isbn")] BookModel kitapModel, HttpPostedFileBase uploadFile) {
            //string isbn = GoruntuIsleme(uploadFile);
            string isbn = _imageProcessing.ProcessImage(uploadFile).Data;
            if (ModelState.IsValid) {
                if (uploadFile != null) {
                    uploadFile.SaveAs(HttpContext.Server.MapPath("~/Images/") + uploadFile.FileName);
                    kitapModel.ImagePath = uploadFile.FileName;
                }
                //var kitapEsit = db.Books.FirstOrDefault(m => m.Isbn == isbn);
                var book = _bookManager.GetByIsbn(isbn).Data;
                //int kitapSayi = db.Books.Where(m => m.UserId == PersonelRoleProvider.personelId1).Count();
                int bookCount = _bookManager.GetListByUser(PersonelRoleProvider.personelId1).Data.Count;
                //if (kitapEsit != null) {
                if (book != null) {
                    //var linq = from p in db.Books
                    //where p.Status == BookStatus.OverdueDate && p.UserId == PersonelRoleProvider.personelId1
                    //select p;
                    var overdueBooks = _bookManager.GetListByUser(PersonelRoleProvider.personelId1).Data
                        .Where(b => b.Status == BookStatus.OverdueDate);
                    int count = overdueBooks.Count();
                    if (count > 0)
                        ViewBag.gecmis = Messages.BookExpired;
                    else {
                        if (bookCount >= 3)
                            ViewBag.Olumsuz2 = Messages.BookExceeded;
                        //else if (db.Books.FirstOrDefault(m => m.Isbn == isbn).Status != BookStatus.Available)
                        else if (_bookManager.GetByIsbn(isbn).Data.Status != BookStatus.Available)
                            ViewBag.Olumsuz2 = Messages.BookInUse;
                        else {
                            ViewBag.Olumlu = Messages.BookReceived;
                            var receivedBook = _bookManager.GetByIsbn(isbn).Data;
                            receivedBook.UserId = PersonelRoleProvider.personelId1;
                            receivedBook.DeliveryTime = DateTime.Now.AddDays(7);//todo::why adding 7 days
                            receivedBook.Status = BookStatus.InUse;
                            _bookManager.Update(book);
                            //db.Books.FirstOrDefault(m => m.Isbn == isbn).UserId = PersonelRoleProvider.personelId1;
                            //db.Books.FirstOrDefault(m => m.Isbn == isbn).DeliveryTime = DateTime.Now.AddDays(7);
                            //db.Books.FirstOrDefault(m => m.Isbn == isbn).Status = BookStatus.InUse;
                            //db.SaveChanges();
                        }
                    }
                } else
                    ViewBag.Olumsuz = Messages.BookNotAvailable;
                return View(kitapModel);
            }
            return View(kitapModel);
        }

        public ActionResult DeliverBook() {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeliverBook([Bind(Include = "Isbn")] BookModel kitapModel, HttpPostedFileBase uploadFile) {
            //string isbn = GoruntuIsleme(uploadFile);
            string isbn = _imageProcessing.ProcessImage(uploadFile).Data;
            if (ModelState.IsValid) {
                if (uploadFile != null) {
                    uploadFile.SaveAs(HttpContext.Server.MapPath("~/Images/") + uploadFile.FileName);
                    kitapModel.ImagePath = uploadFile.FileName;
                }
                //var kitapEsit = db.Books.FirstOrDefault(m => m.Isbn == isbn);
                var book = _bookManager.GetByIsbn(isbn).Data;
                if (book != null && book.UserId == PersonelRoleProvider.personelId1) {
                    ViewBag.Olumlu = Messages.BookReturned;
                    book.UserId = 1;//todo::?
                    book.Status = BookStatus.Available;
                    _bookManager.Update(book);
                    //db.Books.FirstOrDefault(m => m.Isbn == isbn).UserId = 1;
                    //db.Books.FirstOrDefault(m => m.Isbn == isbn).Status = BookStatus.Available;
                    //db.SaveChanges();
                } else
                    ViewBag.Olumsuz = Messages.BookIsNotAccessible;
                return View(kitapModel);
            }
            return View(kitapModel);
        }
    }
}