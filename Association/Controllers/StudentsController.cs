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
using Association.ViewsModel;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Web.Helpers;
using System.Threading.Tasks;
using Association.Security;
using PagedList;

namespace Association.Controllers
{
    [AuthorizeRole]
    public class StudentsController : Controller
    {
        private const string pathImage = "~/Images/Students/";
        private Context db = new Context();

        [AuthorizeRole(Roles="view")]
        public ActionResult AutoCompleteGetParents(string SearchParents)
        {
            var model = db.Parents
                            .Where(p => p.parent_name.StartsWith(SearchParents))
                            .Take(10)
                            .Select(p => new
                            {
                                label =  p.parent_name.ToUpper(),
                                value = p.parent_name.ToUpper() + " " + p.parent_firstName,
                                id = p.parent_id
                            });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeRole(Roles = "view")]
        public ActionResult AutoCompleteName(string SearchString)
        {
            var model = db.Students
                            .Where(s => s.student_name.StartsWith(SearchString))
                            .Take(10)
                            .Select(s => new
                            {
                                label = s.student_name,
                                value = s.student_name.ToUpper() + " " + s.student_firstName
                            });
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        
        // GET: Students
        [HttpGet]
        [AuthorizeRole(Roles = "view")]
        public ActionResult Index(string SearchString, int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var student = from s in db.Students select s;
            

            if (!String.IsNullOrEmpty(SearchString))
            {
                if (SearchString.Contains("*"))
                {
                    SearchString = SearchString.Replace("*", "");
                    student = student.Where(s => s.student_name.Contains(SearchString));
                }
                else
                {
                    student = student.Where(s => s.student_name.Equals(SearchString));
                }
            }
            var students = student.OrderBy(s => s.student_name).ToPagedList(pageNumber, pageSize);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("Partials/_listStudents", students) : View(students); 
            
        }

        // GET: Students/Details/5
        [AuthorizeRole(Roles = "view")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            PopulateAssignedParentData(student);
            if (Request.IsAjaxRequest())
            {
                return PartialView("Details", student);

            }
            //return PartialView("Details", student);
            return View(student);
        }

        // GET: Students/Create
        [AuthorizeRole(Roles = "create,edit")]
        public ActionResult Create()
        {
            var student = new Student();
            student.parents = new List<Parent>();
            PopulateAssignedParentData(student, false);

            return PartialView("Create", student);
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase image, string[] selectedParents, [Bind(Include = "student_id,student_name,student_firstName,student_email,student_type,student_birthday,student_phone,student_mobile,student_otherPhone")] Student student)
        {
            //**************************************************************
            //&& selectedParents.All(p => !string.IsNullOrEmpty(p))
            if (selectedParents != null)
            {
                student.parents = new List<Parent>();
                foreach (var parent in selectedParents)
                {
                    if (parent != "")
                    {
                        var parentToadd = db.Parents.Find(int.Parse(parent));
                        student.parents.Add(parentToadd);
                    }
                }
            }
            bool studentNameExist = db.Students.Any(s => s.student_name == student.student_name);
            bool studentFirstNameExist = db.Students.Any(s => s.student_firstName == student.student_firstName);

            if (ModelState.IsValid)
            {
                if (studentNameExist && studentFirstNameExist)
                {
                    return Json(new { error = true, name = student.student_name.ToUpper() + " " + student.student_firstName });
                }
                else
                {
                    student.student_createDate = DateTime.Now;
                    student.student_UpdateDate = DateTime.Now;
                    db.Students.Add(student);
                    db.SaveChanges();
                    if (image != null && image.ContentLength > 0)
                    {
                        saveImage(image, student);
                    }
                    return Json(new { success = true });

                }
            }
            // if (Request.IsAjaxRequest()) { return PartialView("Index"); }
            PopulateAssignedParentData(student);
            return PartialView("Create", student);
        }

        // GET: Students/Edit/5
        [HttpGet]
        [AuthorizeRole(Roles = "create,edit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students
                .Include(i => i.parents)
                .Where(i => i.student_id == id)
                .Single();

            PopulateAssignedParentData(student, false);

            if (student == null)
            {
                return HttpNotFound();
            }

            //if (Request.IsAjaxRequest())
            //{
            //    return PartialView("Edit", student);

            //}
            return PartialView("Edit", student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string[] selectedParents, HttpPostedFileBase image)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var studentToUpdate = db.Students
                                        .Include(i => i.parents)
                                        .Where(i => i.student_id == id)
                                        .Single();

            if (image != null && image.ContentLength > 0)
            {
                deleteImage(studentToUpdate);
                saveImage(image, studentToUpdate);
            }
            studentToUpdate.student_UpdateDate = DateTime.Now;
            if (TryUpdateModel(studentToUpdate, "", bindStudent))
            {
                try
                {

                    updateStudentParents(selectedParents, studentToUpdate);
                    db.Entry(studentToUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(new { success = true });
                    //return RedirectToAction("Index");

                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Impossible de sauvegarder");
                }

            }
            PopulateAssignedParentData(studentToUpdate, false);


            return PartialView("Edit", studentToUpdate);
            //return RedirectToAction("Index");
        }
        private string[] bindStudent = new string[] { "student_name", "student_sexe", "student_firstName", "student_email", "student_email", "student_type", "student_birthday", "student_phone", "student_mobile", "student_otherPhone", "student_city", "student_postalCode", "student_adress" };

        // GET: Students/Delete/5
        [AuthorizeRole(Roles = "delete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);

            if (student == null)
            {
                return HttpNotFound();
            }
            PopulateAssignedParentData(student);
            if (Request.IsAjaxRequest())
            {
                return PartialView("Delete", student);

            }
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("Delete", student) : View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            if (student.image != null)
            {
                deleteImage(student, true);
            }

            db.Students.Remove(student);
            db.SaveChanges();
            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true });
            }
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

        #region Methodes privées

        // Méthode pour sauvegarder une image
        private void saveImage(HttpPostedFileBase image, Student student)
        {
            var studentID = student.student_id;

            if (studentID != 0)
            {
                var imageFileName = image.FileName;
                var ServerPath = pathImage + studentID + "/";

                var imagePath = Server.MapPath(ServerPath);
                //Vérification de l'existance du répertoire
                if (!Directory.Exists(imagePath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(imagePath); //Création du répertoire
                }

                string saveImageFileName = Path.Combine(imagePath, imageFileName);

                image.SaveAs(saveImageFileName);  //Sauvegarde du nom de l'image originale


                WebImage img = new WebImage(image.InputStream);
                var resizeFileName = "small_" + Path.GetFileName(image.FileName);

                img.Resize(100, img.Height, true, false);
                string saveImageFileNameResize = Path.Combine(imagePath, resizeFileName);
                img.Save(saveImageFileNameResize);

                student.image = ServerPath + imageFileName;
                student.small_image = ServerPath + resizeFileName;
                db.SaveChanges();
            }
        }

        //requete ajax pour supprimer image dans edit
        public ActionResult DeleteImageEdit(int? id)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            var model = db.Students.Find(id);
            deleteImage(model);


            return Json(new { success = true });
            // return RedirectToAction("Edit", "Students", new { id = id });
        }

        //Relation eleve/Parent via class ViewModel
        private void PopulateAssignedParentData(Student student, bool disable = true)
        {
            var allParents = db.Parents;
            var studentParents = new HashSet<int>(student.parents.Select(p => p.parent_id));
            var viewModel = new List<StudentIndexData>();
            foreach (var parent in allParents)
            {
                viewModel.Add(new StudentIndexData
                {
                    ParentID = parent.parent_id,
                    FullName = parent.FullName,
                    Assigned = studentParents.Contains(parent.parent_id)

                });

            }
            ViewBag.Disable = disable;
            ViewBag.Parents = viewModel;
        }

        //Méthode de suppresion de l'image *
        private void deleteImage(Student model, bool directory = false)
        {
            if (model.image != null)
            {
                var pathImageFromStudent = Request.MapPath(pathImage + model.student_id + "/");
                if (directory == false)
                {
                    foreach (string files in Directory.GetFiles(pathImageFromStudent))
                    {
                        System.IO.File.Delete(files);
                    }
                }
                else
                {
                    Directory.Delete(pathImageFromStudent, true);
                }
                model.small_image = null;
                model.image = null;
                db.SaveChanges();
            }
        }

        //Méthode de mise à jour de la relation student - Parent *
        private void updateStudentParents(string[] selectedParents, Student studentToUpdate)
        {

            if (selectedParents == null)
            {
                studentToUpdate.parents = new List<Parent>();
                return;
            }

            var selectedParentsHS = new HashSet<string>(selectedParents);
            var studentParents = new HashSet<int>(studentToUpdate.parents.Select(p => p.parent_id));

            foreach (var parent in db.Parents)
            {
                if (selectedParentsHS.Contains(parent.parent_id.ToString()))
                {
                    if (!studentParents.Contains(parent.parent_id))
                    {
                        studentToUpdate.parents.Add(parent);
                    }

                }
                else
                {
                    if (studentParents.Contains(parent.parent_id))
                    {
                        studentToUpdate.parents.Remove(parent);
                    }
                }
            }

        }
        
        #endregion

    }
}
