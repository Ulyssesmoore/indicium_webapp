using System;
using System.ComponentModel.DataAnnotations;

namespace indicium_webapp.Models
{
    public enum Sex
    {
        M, V, O
    }

    public enum Role
    {
        Lid,
        Bestuur
    }

    public class Member
    {
        public int MemberID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Voornaam mag maximaal 50 karakters.")]
        [Display(Name = "Voornaam")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Achternaam mag maximaal 100 karakters.")]
        [Display(Name = "Achternaam")]
        public string LastName { get; set; }

        [Display(Name = "Geslacht")]
        public Sex Sex { get; set; }

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

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Telefoonnummer")]
        public string PhoneNumber { get; set; }

        [RegularExpression(@"^(NL([0-9]{2})([A-Z]{4})([0-9]{10}))$")]
        [Display(Name = "IBAN nummer")]
        public string Iban { get; set; }

        [Display(Name = "Student nummer")]
        public int StudentNumber { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Startdatum van de studie")]
        public DateTime StartdateStudy { get; set; }

        [Display(Name = "Studietype")]
        public string StudyType { get; set; }

        [Display(Name = "Rol")]
        public Role Role { get; set; }

        [Display(Name = "Registratiedatum")]
        public DateTime RegistrationDate { get; set; }

        [Range(0, 1)]
        [Display(Name = "Is Actief")]
        public int IsActive { get; set; }

        public ApplicationUser MemberAccount { get; set; }
    }
}
