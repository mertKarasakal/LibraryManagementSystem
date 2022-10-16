using System;
using System.Web.Mvc;
using LibraryManagementSystem.WebUI.Business.Abstract;
using LibraryManagementSystem.WebUI.Business.Concrete;
using LibraryManagementSystem.WebUI.Entity.Concrete.Models;
using LibraryManagementSystem.WebUI.Models;
using LibraryManagementSystem.WebUI.Security;

namespace LibraryManagementSystem.WebUI.Controllers {
    public class TimeController : Controller {
        private readonly ITimeService _timeManager;

        public TimeController() {
            _timeManager = new TimeManager();
        }
        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "Time")] TimeModel timeModel) {
            if (ModelState.IsValid) {
                TimeSpan timeSpan = timeModel.Time.Date - DateTime.Now.Date;
                if (timeSpan.Days <= 0)
                    ViewBag.ZamanHatasi = "Zamanı ileri almalısınız!";
                else {
                    var linq = from p in db.Books
                               where p.Status != "BOSTA" && p.DeliveryTime >= DateTime.Now//todo::?hardcoded mı?
                               select p;
                    foreach (var item in linq) {
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