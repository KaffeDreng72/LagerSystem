using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MekDB.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Kode")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Husk denne browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Kodeord")]
        public string Password { get; set; }

        [Display(Name = "Husk Logind?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Dit {0} skal være mindst {2} bogstaver langt.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Kodeord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bekræft Kodeord")]
        [Compare("Password", ErrorMessage = "De to kodeord skal være ens.")]
        public string ConfirmPassword { get; set; }

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

    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Dit {0} skal være mindst {2} bogstaver langt.", MinimumLength = 6)]
        [Display(Name = "Kodeord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "De to kodeord skal være ens.")]
        [Display(Name = "Bekræft Kodeord")]
        public string ConfirmPassword { get; set; }

        public string Kodeord { get; set; } //<- what is this?
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
