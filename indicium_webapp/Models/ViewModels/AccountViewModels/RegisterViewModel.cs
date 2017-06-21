using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using indicium_webapp.Data.ValidationAttributes;
using indicium_webapp.Models.InterfaceItemModels;

namespace indicium_webapp.Models.ViewModels.AccountViewModels
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

        [Required(ErrorMessage = "{0} is verplicht.")]
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
        [MinimumAge(16, ErrorMessage = "De minimumleeftijd is 16 jaar.")]
        [Display(Name = "Geboortedatum")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public string Birthday { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(100, ErrorMessage = "{0} mag maximaal {1} karakter(s) zijn.")]
        [DataType(DataType.Text)]
        [Display(Name = "Straat")]
        public string AddressStreet { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(5, ErrorMessage = "{0} mag maximaal {1} karakter(s) zijn.")]
        [DataType(DataType.Text)]
        [Display(Name = "Huisnummer")]
        public string AddressNumber { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(7, ErrorMessage = "{0} mag maximaal {1} karakter(s) zijn.")]
        [DataType(DataType.Text)]
        [Display(Name = "Postcode")]
        public string AddressPostalCode { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(100, ErrorMessage = "{0} mag maximaal {1} karakter(s) zijn.")]
        [Display(Name = "Woonplaats")]
        public string AddressCity { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [Display(Name = "Land")]
        public string AddressCountry { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(7, ErrorMessage = "Het {0} moet {1} karakters zijn.", MinimumLength = 7)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Het {0} mag alleen bestaan uit nummers.")]
        [DataType(DataType.Text)]
        [Display(Name = "Studentnummer")]
        public string StudentNumber { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Het {0} mag alleen bestaan uit nummers.")]
        [Display(Name = "Beginjaar studie")]
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

        [Display(Name = "Commissie Interesses")]
        public IEnumerable<CheckBoxListItem> Commissions { get; set; }
    }
}
