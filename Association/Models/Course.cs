using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Association.Models
{
    public class Course
    {
        [Key]
        public int course_id { get; set; }
        [Required]
        [Display(Name = "Nom du cours")]
        public string course_name { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime course_datetime { get; set; }



        public virtual ICollection<Category> Category { get; set; }

        //Relation avec Réalisation (one to one)
        [Key, ForeignKey("Realisation")]
        public int realisationID { get; set; }
        public virtual Realisation Realisation { get; set; }

    }
}