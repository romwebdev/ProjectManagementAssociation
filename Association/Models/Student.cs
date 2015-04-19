using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Association.Models
{
    public class Student
    {
        [Key]
        public int student_id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Le nom doit être compris entre 3 et 50 caractères")]
        [Display(Name="Nom")]
        public string student_name { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Le prénom doit être compris entre 3 et 50 caractères")]
        [Display(Name = "Prénom")]
        public string student_firstName { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress] 
        [Display(Name = "Email")]
        public string student_email { get; set; }

        [Display(Name = "Sexe")]
        public bool student_sexe { get; set; }

        [Display(Name = "Fiche")]
        public bool student_type { get; set; }

        [Display(Name = "né le")]
        //[DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime student_birthday { get; set; }

        [StringLength(13, ErrorMessage = "Le numéro de téléphone doit être inférieur à 13 nombres")]
        [RegularExpression(@"^[0-9]{0,13}$", ErrorMessage = "Le numéro doit contenir que des nombres")]
        [Display(Name = "Domicile")]
        public string student_phone { get; set; }

        [StringLength(13, ErrorMessage = "Le numéro de téléphone doit être inférieur à 13 nombres")]
        [RegularExpression(@"^[0-9]{0,13}$", ErrorMessage = "Le numéro doit contenir que des nombres")]
        [Display(Name = "Mobile")]
        public string student_mobile { get; set; }

        [StringLength(13, ErrorMessage = "Le numéro de téléphone doit être inférieur à 13 nombres")]
        [RegularExpression(@"^[0-9]{0,13}$", ErrorMessage = "Le numéro doit contenir que des nombres")]
        [Display(Name = "Pro")]
        public string student_otherPhone { get; set; }

        [StringLength(50, ErrorMessage = "L'adresse doit être inférieur à 50 caractères")]
        [Display(Name = "Adresse")]
        public string student_adress { get; set; }

        [StringLength(30, MinimumLength = 5, ErrorMessage = "La ville doit être compris entre 5 à 30 caractères")]
        [Display(Name = "Ville")]
        public string student_city { get; set; }

        [StringLength(6, MinimumLength = 5, ErrorMessage = "L'adresse doit être compris entre 5 à 6 chiffres")]
        [Display(Name = "Code postal")]
        public string student_postalCode { get; set; }

        [Display(Name="Image")]
        public string image { get; set; }
        public string small_image { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime student_createDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime student_UpdateDate { get; set; }

        [Display(Name = "Elève")]
        public string FullName
        {
            get
            {
                return student_name.ToUpper() + " " + student_firstName;
            }
        }
        [Display(Name = "Photo de l'élève")]
        
        public virtual ICollection<Parent> parents {get; set;}
    }
}