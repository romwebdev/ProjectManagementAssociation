using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IdentitySample.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        // Add the new address properties:
         [Display(Name = "Nom")]
        public string Name { get; set; }
         [Display(Name = "Prénom")]
        public string FirstName { get; set; }
         [Display(Name = "Adresse")]
        public string Address { get; set; }
         [Display(Name = "Ville")]
        public string City { get; set; }

        // Use a sensible display name for views:
        [Display(Name = "Code postal")]
        public string PostalCode { get; set; }
        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}