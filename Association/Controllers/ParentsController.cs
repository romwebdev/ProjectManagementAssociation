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
using Association.Security;
using System.Data.Entity.Infrastructure;
using PagedList;

namespace Association.Controllers
{
    [AuthorizeRole]
    public class ParentsController : Controller
    {
        //Context DataBase
        private Context db = new Context();

        [AuthorizeRole(Roles = "view")]
        public ActionResult AutoCompleteName(string SearchString)
        {
            var model = db.Parents
                            .Where(p => p.parent_name.StartsWith(SearchString))
                            .Take(10)
                            .Select(p => new
                            {
                                label = p.parent_name,
                                value = p.parent_name.ToUpper() + " " + p.parent_firstName
                            });
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        // GET: Parents
        [AuthorizeRole(Roles = "view")]
        public ActionResult Index(string SearchString, int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var parent = from p in db.Parents select p;

            if (!String.IsNullOrEmpty(SearchString))
            {
                if (SearchString.Contains("*"))
                {
                    SearchString = SearchString.Replace("*", "");
                    parent = parent.Where(p => p.parent_name.Contains(SearchString));
                }
                else
                {
                    parent = parent.Where(p => p.parent_name.Equals(SearchString));
                }
            }
            var parents = parent.OrderBy(p => p.parent_name).ToPagedList(pageNumber, pageSize);

            return Request.IsAjaxRequest() ? (ActionResult)PartialView("Partials/_listParents", parents) : View(parents);
        }

        // GET: Parents/Details/5
        [AuthorizeRole(Roles = "view")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("Details", parent);

            }

            return Request.IsAjaxRequest() ? (ActionResult)PartialView("Details", parent) : View(parent);
        }

        // GET: Parents/Create
        [AuthorizeRole(Roles = "create,edit")]
        public ActionResult Create()
        {
            var parents = new Parent();
            return PartialView("Create", parents);
        }

        // POST: Parents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "parent_id,parent_civility,parent_name,parent_firstName,parent_email,parent_phone,parent_mobile,parent_otherPhone,parent_adress,parent_city,parent_postalCode")] Parent parent)
        {
            if (ModelState.IsValid)
            {
                bool parentNameExist = db.Parents.Any(p => p.parent_name == parent.parent_name);
                bool parentFirstNameExist = db.Parents.Any(p => p.parent_firstName == parent.parent_firstName);
                if (parentNameExist && parentFirstNameExist)
                {
                    return Json(new { error = true, name = parent.parent_name.ToUpper() + " " + parent.parent_firstName });
                }
                else
                {
                    parent.parent_createDate = DateTime.Now;
                    parent.parent_UpdateDate = DateTime.Now;
                    db.Parents.Add(parent);
                    db.SaveChanges();
                    return Json(new { success = true, parentName = parent.parent_name.ToUpper() + " " + parent.parent_firstName, parentId = parent.parent_id });
                }

            }

            //return View(parent);
            return PartialView("Create", parent);
        }

        // GET: Parents/Edit/5
        [AuthorizeRole(Roles = "create,edit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }
            return PartialView("Edit", parent);
        }

        // POST: Parents/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            string[] parentBind = new string[] { "parent_civility", "parent_name ", "parent_firstName", "parent_email", "parent_phone", "parent_mobile", "parent_otherPhone", "parent_adress", "parent_city", "parent_postalCode" };
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var parentToUpdate = db.Parents.Find(id);
            parentToUpdate.parent_UpdateDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                if (TryUpdateModel(parentToUpdate, "", parentBind))
                {
                    try
                    {
                        //bool parentNameExist = db.Parents.Any(p => p.parent_name == parentToUpdate.parent_name);
                        //bool parentFirstNameExist = db.Parents.Any(p => p.parent_firstName == parentToUpdate.parent_firstName);
                        //if (parentNameExist && parentFirstNameExist)
                        //{
                        //    return Json(new { error = true, name = parentToUpdate.parent_name.ToUpper() + " " + parentToUpdate.parent_firstName });
                        //}
                        //else
                        //{
                            db.Entry(parentToUpdate).State = EntityState.Modified;
                            db.SaveChanges();
                            return Json(new { success = true });
                        //}
                        
                    }

                    catch (RetryLimitExceededException /* dex */)
                    {
                        //Log the error (uncomment dex variable name and add a line here to write a log.
                        ModelState.AddModelError("", "Impossible de sauvegarder");
                    }

                }
            }
            return PartialView("Edit", parentToUpdate);
        }

        // GET: Parents/Delete/5
        [AuthorizeRole(Roles = "delete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }

            return Request.IsAjaxRequest() ? (ActionResult)PartialView("Delete", parent) : View(parent);
        }

        // POST: Parents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Parent parent = db.Parents.Find(id);
            db.Parents.Remove(parent);
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
