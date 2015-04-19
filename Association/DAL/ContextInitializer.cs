using Association.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Association.DAL
{
    public class ContextInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<Context>
    {
        protected override void Seed(Context context)
        {
            Parent parent01 = new Parent
            {
                parent_name = "Romeo",
                parent_firstName = "Double Romeo",
                parent_email = "romeo-d-9878@ex.com",
                parent_otherPhone = "0490903458",
                parent_phone = "0490858587",
                parent_mobile = "0650505050",
                parent_adress = "12 rue de la république",
                parent_city = "Orange",
                parent_postalCode = "84100",
                parent_civility = Civility.Père,
                parent_createDate = DateTime.Now,
                parent_UpdateDate = DateTime.Now
            };
            Parent parent02 = new Parent
            {
                parent_name = "Juliette",
                parent_firstName = "Vici",
                parent_email = "juliette_p08@ex.com",
                parent_otherPhone = "0490326598",
                parent_phone = "0991848478",
                parent_mobile = "0754266548",
                parent_adress = "156 avenue des courèges",
                parent_city = "Orange",
                parent_postalCode = "84100",
                parent_civility = Civility.Mère,
                parent_createDate = DateTime.Now,
                parent_UpdateDate = DateTime.Now
            };

            Parent parent03 = new Parent
            {
                parent_name = "Nicoste",
                parent_firstName = "Alfredo",
                parent_email = "alfredo_1500@ex.com",
                parent_otherPhone = "",
                parent_phone = "",
                parent_mobile = "0745897898",
                parent_adress = "4 av du 11 novembre",
                parent_city = "Orange",
                parent_postalCode = "84100",
                parent_civility = Civility.Père,
                parent_createDate = DateTime.Now,
                parent_UpdateDate = DateTime.Now
            };

            Parent parent04 = new Parent
            {
                parent_name = "Kolopes",
                parent_firstName = "Sarah",
                parent_email = "sarah-kolopes@ex.com",
                parent_otherPhone = "0456548547",
                parent_phone = "0432154585",
                parent_mobile = "",
                parent_adress = "45 rue jean jaures",
                parent_city = "Avignon",
                parent_postalCode = "84000",
                parent_civility = Civility.Mère,
                parent_createDate = DateTime.Now,
                parent_UpdateDate = DateTime.Now
            };
            Parent parent05 = new Parent
            {
                parent_name = "Kolopes",
                parent_firstName = "Paul",
                parent_email = "paul-kalopes@ex.com",
                parent_otherPhone = "",
                parent_phone = "0432154585",
                parent_mobile = "0645515252",
                parent_adress = "45 rue jean jaures",
                parent_city = "Avignon",
                parent_postalCode = "84000",
                parent_civility = Civility.Père,
                parent_createDate = DateTime.Now,
                parent_UpdateDate = DateTime.Now
            };
            var parents = new List<Parent> { parent01, parent02, parent03, parent04, parent05 };
            parents.ForEach(p => context.Parents.Add(p));
            context.SaveChanges();

            var students = new List<Student>
            {
                new Student{ student_sexe=true,
                             student_name="Nicoste", 
                             student_firstName ="Rodolphe",
                             student_birthday=DateTime.Parse("1998-06-15"),
                             student_email="rodolph.ly@example.com",
                             student_mobile="0605020300",
                             student_type= false,
                             student_phone= "",
                             student_otherPhone="",
                             student_createDate = DateTime.Now,
                             student_UpdateDate = DateTime.Now,
                             parents = new List<Parent>{parent03, parent05},
                            },

                new Student{ student_sexe=true,
                             student_name="Romeo", 
                             student_firstName ="Nicolas",
                             student_birthday=DateTime.Parse("1999-12-12"),
                             student_email="n.jul1212@example.com",
                             student_mobile="0789854852",
                             student_type= false,
                             student_phone= "",
                             student_otherPhone="",
                             student_createDate = DateTime.Now,
                             student_UpdateDate = DateTime.Now,
                             parents = new List<Parent>{parent01},

                            },
                new Student{ student_sexe=false,
                             student_name="Nicoste", 
                             student_firstName ="lola",
                             student_birthday=DateTime.Parse("1990-02-15"),
                             student_email="l.Kouala-45878@example.com",
                             student_mobile="0756251645",
                             student_type= true,
                             student_phone= "",
                             student_otherPhone="",
                             student_createDate = DateTime.Now,
                             student_UpdateDate = DateTime.Now,
                             parents = new List<Parent> {parent03},
                            },

            };
            students.ForEach(s => context.Students.Add(s));
            context.SaveChanges();




            var categories = new List<Category>
            {
                new Category {category_name="Jazz 1"},
                new Category {category_name="Jazz 2"},
                new Category {category_name="Jazz 3"},
                new Category {category_name="Jazz 4"},
                new Category {category_name="Eveil"},
                new Category {category_name="hip hop 1"},
                new Category {category_name="hip hop 2"},
                new Category {category_name="hip hop 3"},
                new Category {category_name="Classique 1"},
                new Category {category_name="Classique 2"},
                new Category {category_name="Classique 3"},

            };
            categories.ForEach(c => context.Categories.Add(c));
            context.SaveChanges();


        }

    }
}