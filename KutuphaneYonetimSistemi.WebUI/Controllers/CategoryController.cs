using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using LibraryManagementSystem.WebUI.Business.Abstract;
using LibraryManagementSystem.WebUI.Business.Concrete;
using LibraryManagementSystem.WebUI.Entity;
using LibraryManagementSystem.WebUI.Entity.Concrete;
using LibraryManagementSystem.WebUI.Models.EntityFramework;

namespace LibraryManagementSystem.WebUI.Controllers {
    [Authorize(Roles = "1")]
    public class CategoryController : Controller {
        private readonly ICategoryService _categoryManager;

        public CategoryController() {
            _categoryManager = new CategoryManager();
        }

        public ActionResult Index() {
            //todo::remove::return View(db.Categories.ToList());
            return View(_categoryManager.GetList().Data);
        }

        public ActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Category category) {
            if (ModelState.IsValid) {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        public ActionResult Edit(int? id) {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Category category = db.Categories.Find(id);
            if (category == null)
                return HttpNotFound();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Category category) {
            if (ModelState.IsValid) {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public ActionResult Delete(int? id) {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Category category = db.Categories.Find(id);
            if (category == null)
                return HttpNotFound();
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing) {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
