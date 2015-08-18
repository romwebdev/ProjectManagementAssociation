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

namespace Association.Controllers
{
    public class CoursesController : Controller
    {
        private Context db = new Context();

        // GET: Courses
        public ActionResult getCourseCalendar(int? selectRealisation )
        {
            var courses = db.Courses.ToArray().Select(c => new
            {
                
                title = c.course_name,
                start =  c.course_startTime.ToShortTimeString() ,
                end = c.course_endTime.ToShortTimeString(),
                test = c.course_day,
                dow = "[" + (int)c.course_day + "]",
                //backgroundColor = 'grey'
                //,
                //start: '13:00',
                //end: '16:00',
                //dow: [1],
                //backgroundColor: 'grey'
            });
            //var eventlist = courses.ToArray();
            return Json(courses, JsonRequestBehavior.AllowGet);
        } 
        public ActionResult Index()
        {
            var courses = db.Courses
                            .Include(c => c.Category)
                            .Include(r => r.Realisation).ToList();

            return View(courses);
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            PopulateCategoriesDropDownList();
            PopulateRealisationsDropDownList();
            return View();
        }

        // POST: Courses/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "course_id,course_name,course_startTime,course_endTime,course_day, categoryID, realisationID")] Course course)
        {
            if (ModelState.IsValid)
            {
           
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateCategoriesDropDownList(course.categoryID);
            PopulateRealisationsDropDownList(course.realisationID);
           
            return View(course);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            PopulateCategoriesDropDownList(course.categoryID);
            PopulateRealisationsDropDownList(course.realisationID);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "course_id,course_name,course_startTime,course_endTime,course_day, categoryID, realisationID")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateCategoriesDropDownList(course.categoryID);
            PopulateRealisationsDropDownList(course.realisationID);
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void PopulateCategoriesDropDownList(object selectedCategory = null)
        {
            var categoryQuery = from d in db.Categories
                                   orderby d.cat_name
                                   select d;
            ViewBag.categoryID = new SelectList(categoryQuery, "cat_id", "cat_name", selectedCategory);
        }

        private void PopulateRealisationsDropDownList(object selectedRealisation = null)
        {
            var realisationQuery = from r in db.Realisations
                                orderby r.rea_name
                                select r;
            ViewBag.realisationID = new SelectList(realisationQuery, "rea_id", "rea_name", selectedRealisation);
        }
    }
}
