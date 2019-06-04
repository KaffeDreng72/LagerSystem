using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MekDB.Models;

namespace MekDB.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Role Name")]
        public string Name { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Fornavn")]
        [StringLength(50)]
        public string Fornavn { get; set; }

        [Required]
        [Display(Name = "Efternavn")]
        [StringLength(50)]
        public string Efternavn { get; set; }

        [Required]
        [Display(Name = "Hold")]
        [StringLength(50)]
        public string Hold { get; set; }

        [Required]
        [Display(Name = "Kontakt Listen")]
        [StringLength(50)]
        public string KontaktListen { get; set; }

        [Display(Name = "Telefon Nummer")]
        public Address Address { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}