using LibraryManagementSystem.WebUI.Models.EntityFramework;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace LibraryManagementSystem.WebUI.Controllers
{
    public class SecurityController : Controller
    {
        private LibraryContext db = new LibraryContext();

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [System.Obsolete]
        public ActionResult Login(User user)
        {
            var personelInDb = db.Users.FirstOrDefault(x => x.Username == user.Username && x.Password == user.Password);
            if (personelInDb != null)
            {
                FormsAuthentication.SetAuthCookie(personelInDb.Username, false);
                return RedirectToAction("Index", "Book");
            }
            else
            {
                ViewBag.Mesaj = "Lütfen bilgilerinizi kontrol ediniz!";
                return View();
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}