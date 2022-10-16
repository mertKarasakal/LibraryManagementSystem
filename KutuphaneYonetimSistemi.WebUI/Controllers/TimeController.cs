using LibraryManagementSystem.WebUI.Models;
using LibraryManagementSystem.WebUI.Models.EntityFramework;
using LibraryManagementSystem.WebUI.Security;
using System;
using System.Linq;
using System.Web.Mvc;

namespace KutupLibraryManagementSystemhaneYonetimSistemi.WebUI.Controllers
{
    public class TimeController : Controller
    {
        private LibraryContext db = new LibraryContext();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "Time")] TimeModel timeModel)
        {
            if (ModelState.IsValid)
            {
                TimeSpan timeSpan = timeModel.Time.Date - DateTime.Now.Date;
                if (timeSpan.Days <= 0)
                    ViewBag.ZamanHatasi = "Zamanı ileri almalısınız!";
                else
                {
                    var linq = from p in db.Books
                               where p.Status != "BOSTA" && p.DeliveryTime >= DateTime.Now//todo::?hardcoded mı?
                               select p;
                    foreach (var item in linq)
                    {
                        db.Books.FirstOrDefault(k => k.Id == item.Id).Status = "teslimtarihigecmis";//todo::?hardcoded mı?
                    }
                    db.SaveChanges();
                    PersonelRoleProvider.MevcutGun = timeModel.Time;
                    return RedirectToAction("Index");
                }
            }
            return View(timeModel);
        }
    }
}