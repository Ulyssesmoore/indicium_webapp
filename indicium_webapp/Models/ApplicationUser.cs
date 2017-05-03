using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace indicium_webapp.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {

            SignUps = new HashSet<SignUp>();
        }

        [Required]
        [StringLength(50, ErrorMessage = "Voornaam mag maximaal 50 karakters.")]
        [Display(Name = "Voornaam")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Achternaam mag maximaal 100 karakters.")]
        [Display(Name = "Achternaam")]
        public string LastName { get; set; }

        [Display(Name = "Geslacht")]
        [RegularExpression(@"^(M|V|O)$")]
        public string Sex { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Geboortedatum")]
        public DateTime Birthday { get; set; }

        [Required]
        [Display(Name = "Straat")]
        public string AddressStreet { get; set; }

        [Required]
        [Display(Name = "Huisnummer")]
        public string AddressNumber { get; set; }

        [Required]
        [Display(Name = "Postcode")]
        public string AddressPostalCode { get; set; }

        [Required]
        [Display(Name = "Plaats")]
        public string AddressCity { get; set; }

        [Required]
        [Display(Name = "Land")]
        public string AddressCountry { get; set; }

        [RegularExpression(@"^(NL([0-9]{2})([A-Z]{4})([0-9]{10}))$")]
        [Display(Name = "IBAN nummer")]

        public string Iban { get; set; }

        [Display(Name = "Student nummer")]
        public int StudentNumber { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Startdatum studie")]
        public DateTime StartdateStudy { get; set; }

        [Display(Name = "Studietype")]
        public string StudyType { get; set; }   

        [Display(Name = "Registratiedatum")]
        public DateTime RegistrationDate { get; set; }

        [Range(0, 1)]
        [Display(Name = "Is Actief")]
        public int IsActive { get; set; }

        [Range(0, 2)]
        [Display(Name = "Is Goedgekeurd")]
        public int IsApproved { get; set; }

        public virtual ICollection<SignUp> SignUps { get; set; }
    }
}
