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

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        [DataType(DataType.Time)]
        [Display(Name = "Heure début")]
        public DateTime course_startTime { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        [DataType(DataType.Time)]
        [Display(Name = "Heure fin")]
        public DateTime course_endTime { get; set; }

        public enum Day
        {
            Lundi = 1,
            Mardi = 2,
            Mercredi = 3,
            Jeudi = 4,
            Vendredi = 5,
            Samedi = 6,
            Dimanche = 7
        }

        [Display(Name = "Jour")]
        public Day course_day { get; set; }

        [ForeignKey("categoryID")]
        public Category Category { get; set; }
        [Display(Name = "Catégorie")]
        public int categoryID { get; set; }

        [ForeignKey("realisationID")]
        public Realisation Realisation { get; set; }

        [Display(Name = "Date de réalisation")]
        public int realisationID { get; set; }

        //Relation avec Réalisation (one to one)
        //[Key, ForeignKey("Realisation")]
        //public int realisationID { get; set; }
        //public Realisation Realisation { get; set; }

    }
}