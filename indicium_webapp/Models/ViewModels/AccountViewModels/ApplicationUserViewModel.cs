using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using indicium_webapp.Models.InterfaceItemModels;

namespace indicium_webapp.Models.ViewModels.AccountViewModels
{
    public class ApplicationUserViewModel
    {
        public ApplicationUserViewModel()
        {
            Roles = new HashSet<CheckBoxListItem>();
        }
        
        public string Id { get; set; }
        
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

        [Display(Name = "Naam")]
        public string Name
        {
            get { return FirstName + " " + LastName; }
        }
        
        [EmailAddress]
        [Display(Name = "E-mailadres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(1)]
        [DataType(DataType.Text)]
        [Display(Name = "Geslacht")]
        public string Sex { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(100, ErrorMessage = "{0} mag maximaal {1} karakter(s) zijn.")]
        [DataType(DataType.Date)]
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
        [RegularExpression(@"^[0-9]{4}\s?[a-zA-Z]{2}$", ErrorMessage = "Deze invoer is geen postcode")]
        [DataType(DataType.Text)]
        [Display(Name = "Postcode")]
        public string AddressPostalCode { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [Display(Name = "Woonplaats")]
        public string AddressCity { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [Display(Name = "Land")]
        public string AddressCountry { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(7, ErrorMessage = "Het {0} moet {1} karakters zijn.", MinimumLength = 7)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Ingave mag alleen nummers bevatten.")]
        [DataType(DataType.Text)]
        [Display(Name = "Studentnummer")]
        public string StudentNumber { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [Display(Name = "Beginjaar studie")]
        [Range(2010, 2030)]
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

        public DateTime RegistrationDate { get; set; }

        public Status Status { get; set; }

        public String UserName { get; set; }

        [Display(Name = "Rollen")]
        public IEnumerable<CheckBoxListItem> Roles { get; set; }

    }
}
