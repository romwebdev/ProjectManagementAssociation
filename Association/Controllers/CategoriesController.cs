using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Association.DAL;
using Association.Models;
using System.Data.Entity.Infrastructure;
using Association.Security;

namespace Association.Controllers
{
    [AuthorizeRole]
    public class CategoriesController : Controller
    {
        private Context db = new Context();

        // GET: Categories
        [AuthorizeRole(Roles = "view")]
        public ActionResult Index()
        {
            var categories = db.Categories.ToList();
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("Index", categories) : View(categories);
        }

        // GET: Categories/Details/5
        [AuthorizeRole(Roles = "view")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("Details", category) : View(category);
        }

        // GET: Categories/Create
        [AuthorizeRole(Roles = "create,edit")]
        public ActionResult Create()
        {
            return PartialView("Create");
        }

        // POST: Categories/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cat_id,cat_name, cat_UpdateDate, cat_createDate")] Category category)
        {

            if (ModelState.IsValid)
            {
                category.cat_createDate = DateTime.Now;
                category.cat_UpdateDate = DateTime.Now;

                bool NameExist = db.Categories.Any(c => c.cat_name == category.cat_name);
                if (NameExist)
                {
                    return Json(new { error = true, name = category.cat_name.ToUpper() });
                }
                else
                {
                    db.Categories.Add(category);
                    db.SaveChanges();
                    return Json(new { success = true });
                }

            }

            return PartialView("Create", category);
        }

        // GET: Categories/Edit/5
        [AuthorizeRole(Roles = "create,edit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return PartialView("Edit", category);
        }

        // POST: Categories/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            string[] CategoryBind = new string[] { "cat_name" };
            var catToUpdate = db.Categories.Find(id);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                if (TryUpdateModel(catToUpdate, "", CategoryBind))
                {
                    try
                    {
                        catToUpdate.cat_UpdateDate = DateTime.Now;
                        bool NameExist = db.Categories.Any(c => c.cat_name == catToUpdate.cat_name);
                        if (NameExist)
                        {
                            return Json(new { error = true, name = catToUpdate.cat_name.ToUpper() });
                        }
                        else
                        {
                            db.Entry(catToUpdate).State = EntityState.Modified;
                            db.SaveChanges();
                            return Json(new { success = true });
                        }

                    }
                    catch (RetryLimitExceededException)
                    {
                        ModelState.AddModelError("", "Impossible de sauvegarder");
                    }
                }

            }
            return PartialView("Edit", catToUpdate);
        }

        // GET: Categories/Delete/5
        [AuthorizeRole(Roles = "delete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("Delete", category) : View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();

            return Request.IsAjaxRequest() ? (JsonResult)Json(new { success = true }) : (ActionResult)RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
