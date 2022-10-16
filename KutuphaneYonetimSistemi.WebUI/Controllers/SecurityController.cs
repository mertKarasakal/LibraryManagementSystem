using LibraryManagementSystem.WebUI.Models.EntityFramework;
using System.Web.Mvc;
using System.Web.Security;
using LibraryManagementSystem.WebUI.Business.Abstract;
using LibraryManagementSystem.WebUI.Business.Concrete;
using LibraryManagementSystem.WebUI.Entity;
using LibraryManagementSystem.WebUI.Entity.Concrete;

namespace LibraryManagementSystem.WebUI.Controllers {
    public class SecurityController : Controller {
        private readonly ISecurityService _securityManager;

        public SecurityController() {
            _securityManager = new SecurityManager();
        }

        [AllowAnonymous]
        public ActionResult Login() {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [System.Obsolete]
        public ActionResult Login(User user) {
            var personelInDb = db.Users.FirstOrDefault(x => x.Username == user.Username && x.Password == user.Password);
            if (personelInDb != null) {
                FormsAuthentication.SetAuthCookie(personelInDb.Username, false);
                return RedirectToAction("Index", "Book");
            } else {
                ViewBag.Mesaj = "Lütfen bilgilerinizi kontrol ediniz!";
                return View();
            }
        }

        public ActionResult Logout() {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}