using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(50, ErrorMessage = "{0} mag maximaal {1} karakter(s) zijn.")]
        [DataType(DataType.Text)]
        [Display(Name = "Voornaam")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(100, ErrorMessage = "{0} mag maximaal {1} karakter(s) zijn.")]
        [DataType(DataType.Text)]
        [Display(Name = "Achternaam")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "{0} is verplicht.")]
        [EmailAddress]
        [Display(Name = "E-mailadres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(100, ErrorMessage = "Het {0} moet minimaal {2} en maximaal {1} karakters lang zijn.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bevestig wachtwoord")]
        [Compare("Password", ErrorMessage = "Het wachtwoord en bevestigingsveld komen niet met elkaar overeen.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(1, ErrorMessage = "{0} mag maximaal {1} karakter(s) zijn.")]
        [DataType(DataType.Text)]
        [Display(Name = "Geslacht")]
        public string Sex { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(100, ErrorMessage = "{0} mag maximaal {1} karakter(s) zijn.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Geboortedatum")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public string Birthday { get; set; }

        [StringLength(100, ErrorMessage = "{0} mag maximaal {1} karakter(s) zijn.")]
        [DataType(DataType.Text)]
        [Display(Name = "Straat")]
        public string AddressStreet { get; set; }

        [StringLength(5, ErrorMessage = "{0} mag maximaal {1} karakter(s) zijn.")]
        [DataType(DataType.Text)]
        [Display(Name = "Huisnummer")]
        public string AddressNumber { get; set; }

        [StringLength(7, ErrorMessage = "{0} mag maximaal {1} karakter(s) zijn.")]
        [DataType(DataType.Text)]
        [Display(Name = "Postcode")]
        public string AddressPostalCode { get; set; }

        [StringLength(100, ErrorMessage = "{0} mag maximaal {1} karakter(s) zijn.")]
        [Display(Name = "Woonplaats")]
        public string AddressCity { get; set; }

        [Display(Name = "Land")]
        public string AddressCountry { get; set; }

        [RegularExpression(@"^(NL([0-9]{2})([A-Z]{4})([0-9]{10}))$", ErrorMessage = "Ongeldige ingave")]
        [Display(Name = "IBAN")]
        public string Iban { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(7, ErrorMessage = "Het {0} moet minimaal {2} en maximaal {1} karakters lang zijn.", MinimumLength = 7)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Must be numeric")]
        [DataType(DataType.Text)]
        [Display(Name = "Studentnummer")]
        public string StudentNumber { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Begindatum studie")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd'-'MM'-'yyyy}")]
        public string StartdateStudy { get; set; }
        
        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(30, ErrorMessage = "{0} mag maximaal {1} karakter(s) zijn.")]
        [DataType(DataType.Text)]
        [Display(Name = "Studietype")]
        public string StudyType { get; set; }

        [StringLength(100, ErrorMessage = "{0} mag maximaal {1} karakter(s) zijn.")]
        [DataType(DataType.Text)]
        [Display(Name = "Telefoonnummer")]
        public string PhoneNumber { get; set; }

    }
}
