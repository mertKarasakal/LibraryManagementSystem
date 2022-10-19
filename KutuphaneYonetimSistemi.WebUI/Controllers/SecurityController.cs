using System.Web.Mvc;
using LibraryManagementSystem.WebUI.Business.Abstract;
using LibraryManagementSystem.WebUI.Business.Concrete;
using LibraryManagementSystem.WebUI.Entity.Concrete;
using LibraryManagementSystem.WebUI.Utilities.Constants;

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
            if (_securityManager.Login(user).Success) {
                return RedirectToAction("Index", "Book");
            } else {
                ViewBag.Message = Messages.CheckYourInformation;
                return View();
            }
        }

        public ActionResult Logout() {
            _securityManager.Logout();
            return RedirectToAction("Login");
        }
    }
}