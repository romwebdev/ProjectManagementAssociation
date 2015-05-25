using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Association.Models
{
    public class Category
    {
        [Key]
        public int cat_id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Le nom doit être compris entre 3 et 20 caractères")]
        [Display(Name = "Nom")]
        public string cat_name { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date de création")]
        public DateTime cat_createDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date de modification")]
        public DateTime cat_UpdateDate { get; set; }

    }
}