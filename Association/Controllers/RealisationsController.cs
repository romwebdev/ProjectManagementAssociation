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

namespace Association.Controllers
{
    public class RealisationsController : Controller
    {
        private Context db = new Context();

        // GET: Realisations
        public ActionResult Index()
        {
            var realisation = db.Realisations.ToList();
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("Partials/_listRealisations", realisation) : View(realisation);

        }

        // GET: Realisations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Realisation realisation = db.Realisations.Find(id);
            if (realisation == null)
            {
                return HttpNotFound();
            }

            return Request.IsAjaxRequest() ? (ActionResult)PartialView("Details", realisation) : View(realisation);
        }

        // GET: Realisations/Create
        public ActionResult Create()
        {
            return PartialView("Create");
        }

        // POST: Realisations/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "rea_id,rea_name,rea_dateFirst,rea_dateLast,rea_createDate,rea_UpdateDate")] Realisation realisation)
        {
            if (ModelState.IsValid)
            {
                var yearFirst = realisation.rea_dateFirst.Year.ToString();
                var yearLast = realisation.rea_dateLast.Year.ToString();
                realisation.rea_name = "Année " + yearFirst + "-" + yearLast;
                realisation.rea_createDate = DateTime.Now;
                realisation.rea_UpdateDate = DateTime.Now;

                bool NameExist = db.Realisations.Any(r => r.rea_name == realisation.rea_name);
                if (NameExist)
                {
                    return Json(new { error = true, name = realisation.rea_name.ToUpper() });
                }
                else
                {
                    db.Realisations.Add(realisation);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }

            return PartialView("Create", realisation);
        }

        // GET: Realisations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Realisation realisation = db.Realisations.Find(id);
            if (realisation == null)
            {
                return HttpNotFound();
            }
            return PartialView("Edit", realisation);
        }

        // POST: Realisations/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            string[] RealisationBind = new string[] { "rea_name", "rea_dateFirst", "rea_dateLast" };
            var realToUpdate = db.Realisations.Find(id);




            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            if (ModelState.IsValid)
            
            {
                if (TryUpdateModel(realToUpdate, "", RealisationBind))
                {
                    try
                    {
                        var yearFirst = realToUpdate.rea_dateFirst.Year.ToString();
                        var yearLast = realToUpdate.rea_dateLast.Year.ToString();

                        realToUpdate.rea_name = "Année " + yearFirst + "-" + yearLast;
                        realToUpdate.rea_UpdateDate = DateTime.Now;

                        bool NameExist = db.Realisations.Any(r => r.rea_name == realToUpdate.rea_name);
                        if (NameExist)
                        {
                            return Json(new { error = true, name = realToUpdate.rea_name.ToUpper() });
                        }
                        else
                        {
                            db.Entry(realToUpdate).State = EntityState.Modified;
                            db.SaveChanges();
                            return Json(new { success = true });
                        }


                        
                        //return RedirectToAction("Index");
                    }
                    catch (RetryLimitExceededException)
                    {
                        ModelState.AddModelError("", "Impossible de sauvegarder");
                    }
                }


            }
            return PartialView("Edit", realToUpdate);
        }

        // GET: Realisations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Realisation realisation = db.Realisations.Find(id);
            if (realisation == null)
            {
                return HttpNotFound();
            }
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("Delete", realisation) : View(realisation);
        }

        // POST: Realisations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Realisation realisation = db.Realisations.Find(id);
            db.Realisations.Remove(realisation);
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
