using Association.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Association.Models
{
    public enum Civility
    {
        Père,
        Mère,
        Grand_père,
        Grand_mère,
        Frère,
        Soeur,
        Autre
    }
    public class Parent
    {
        [Key]
        public int parent_id { get; set; }

        [Required]
        [Display(Name = "Civilité")]
        [EnumDataType(typeof(Civility))]
        public Civility parent_civility { get; set; }

        [Required]
        [Display(Name = "Nom")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Le nom doit être compris entre 3 et 50 caractères")]
        public string parent_name { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Le prénom doit être compris entre 3 et 50 caractères")]
        [Display(Name = "Prénom")]
        public string parent_firstName { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string parent_email { get; set; }


        [StringLength(13, ErrorMessage = "Le numéro de téléphone doit être inférieur à 13 nombres")]
        [RegularExpression(@"^[0-9]{0,13}$", ErrorMessage = "Le numéro doit contenir que des nombres")]
        [Display(Name = "Fixe")]
        public string parent_phone { get; set; }

        [StringLength(13, ErrorMessage = "Le numéro de téléphone doit être inférieur à 13 nombres")]
        [RegularExpression(@"^[0-9]{0,13}$", ErrorMessage = "Le numéro doit contenir que des nombres")]
        [Display(Name = "Mobile")]
        public string parent_mobile { get; set; }

        [StringLength(13, ErrorMessage = "Le numéro de téléphone doit être inférieur à 13 nombres")]
        [RegularExpression(@"^[0-9]{0,13}$", ErrorMessage = "Le numéro doit contenir que des nombres")]
        [Display(Name = "Autre")]
        public string parent_otherPhone { get; set; }

        [StringLength(50, ErrorMessage = "L'adresse doit être inférieur à 50 caractères")]
        [Display(Name = "Adresse")]
        public string parent_adress { get; set; }

        [StringLength(30, MinimumLength = 5, ErrorMessage = "La ville doit être compris entre 5 à 30 caractères")]
        [Display(Name = "Ville")]
        public string parent_city { get; set; }

        [StringLength(6, MinimumLength = 5, ErrorMessage = "L'adresse doit être compris entre 5 à 6 chiffres")]
        [Display(Name = "Code postal")]
        public string parent_postalCode { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime parent_createDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime parent_UpdateDate { get; set; }

        [Display(Name = "Parent")]
        public string FullName
        {
            get
            {
                return parent_name.ToUpper() + " " + parent_firstName;
            }
        }
        public virtual ICollection<Student> Students { get; set; }


    }
}