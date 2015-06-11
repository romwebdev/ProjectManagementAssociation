using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Association.Models
{
    public class Realisation : IValidatableObject
    {
        [Key]
        public int rea_id { get; set; }

        [Display(Name = "Nom")]
        public string rea_name { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date de début")]
        public DateTime rea_dateFirst { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date de fin")]
        public DateTime rea_dateLast { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date de création")]
        public DateTime rea_createDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date de modification")]
        public DateTime rea_UpdateDate { get; set; }

        public virtual Course Course { get; set; } 

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (rea_dateLast < rea_dateFirst)
            {
                yield return new ValidationResult("La date de début doit être supérieure à la date de fin");
            }
        }

    }
   
}