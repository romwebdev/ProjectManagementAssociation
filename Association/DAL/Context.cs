using Association.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Association.DAL
{
    public class Context : DbContext
    {
        public Context()
            : base("Context")
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Realisation> Realisations { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Convention
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();


            modelBuilder.Entity<Student>()
                .HasMany(s => s.parents)
                .WithMany(p => p.Students)
                .Map(t =>
                    {
                        t.MapLeftKey("student_id");
                        t.MapRightKey("parent_id");
                        t.ToTable("StudentParent");
                    });


            ////modelBuilder.Entity<Realisation>()
            ////    .HasRequired(c => c.Course).WithRequiredDependent(r => r.Realisation);
            //    //.HasOptional(r => r.Course)
            //    //.WithRequired(r => r.Realisation);

            //modelBuilder.Entity<Course>().HasKey(c => c.realisationID).HasRequired(r => r.Realisation).WithRequiredDependent(c => c.Course);
            ////modelBuilder.Entity<Course>()
            ////    .HasKey(c => c.realisationID)
            ////    .HasRequired<Category>(cat => cat.Category)
            ////    .WithMany(cat => cat.Course)
            ////    .HasForeignKey(cat => cat.catID);

            //modelBuilder.Entity<Course>().HasKey(c => c.course_id);

            //modelBuilder.Entity<Category>().HasKey(cat => cat.cat_id).HasRequired(c => c.Course).WithRequiredDependent(ca => ca.Category);

            ////modelBuilder.Entity<Course>()
            ////   .HasKey(c => c.catID)
            ////   .HasRequired(c => c.Category).WithRequiredDependent(c => c.Course);



        }
    }
}