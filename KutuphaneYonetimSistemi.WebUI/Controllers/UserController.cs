using System.Net;
using System.Web.Mvc;
using LibraryManagementSystem.WebUI.Business.Abstract;
using LibraryManagementSystem.WebUI.Business.Concrete;
using LibraryManagementSystem.WebUI.Entity.Concrete;

namespace LibraryManagementSystem.WebUI.Controllers {
    [Authorize(Roles = "1,2")]
    public class UserController : Controller {
        private readonly IUserService _userManager;

        public UserController() {
            _userManager = new UserManager();
        }

        [Authorize(Roles = "1")]
        public ActionResult Index() {
            //return View(db.Users.ToList());
            return View(_userManager.GetList().Data);
        }

        [Authorize(Roles = "1")]
        public ActionResult Create() {
            return View();
        }
        [Authorize(Roles = "1")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Surname,Role,Username,Password")] User user) {
            if (ModelState.IsValid) {
                //db.Users.Add(user);
                //db.SaveChanges();
                _userManager.Add(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [Authorize(Roles = "1")]
        // GET: User/Edit/5
        public ActionResult Edit(int id) {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //User user = db.Users.Find(id);
            User user = _userManager.GetById(id).Data;
            if (user == null)
                return HttpNotFound();
            return View(user);
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Surname,Role,Username,Password")] User user) {
            if (ModelState.IsValid) {
                //db.Entry(user).State = EntityState.Modified;
                //db.SaveChanges();
                _userManager.Update(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [Authorize(Roles = "1")]
        public ActionResult Delete(int id) {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //User user = db.Users.Find(id);
            User user = _userManager.GetById(id).Data;
            if (user == null)
                return HttpNotFound();
            return View(user);
        }

        [Authorize(Roles = "1")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            //User user = db.Users.Find(id);
            User user = _userManager.GetById(id).Data;
            //db.Users.Remove(user);
            //db.SaveChanges();
            _userManager.Delete(user);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }

        //todo::do-en
        public ActionResult HesapAyarlari(int id) {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //User user = db.Users.Find(id);
            User user = _userManager.GetById(id).Data;
            if (user == null)
                return HttpNotFound();
            return View(user);
        }


        //todo::do-en
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HesapAyarlari([Bind(Include = "Id,Name,Surname,Role,Username,Password")] User user) {
            if (ModelState.IsValid) {
                //db.Entry(user).State = EntityState.Modified;
                //db.SaveChanges();
                _userManager.Update(user);
                return RedirectToAction("Index", "Book");
            }
            return View(user);
        }
    }
}