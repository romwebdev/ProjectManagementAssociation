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
        public Context() : base("Context")
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
                        
               

        
        }
    }
}