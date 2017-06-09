using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage ="This first name is invalid")]
        [DataType(DataType.Text)]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "This name is invalid")]
        [DataType(DataType.Text)]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "This name is invalid")]
        [DataType(DataType.Date)]
        [Display(Name = "Birthday")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd'-'MM'-'yyyy}")]
        public string Birthday { get; set; }

        [Required]
        [StringLength(1)]
        [DataType(DataType.Text)]
        [Display(Name = "Sex")]
        public string Sex { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "StartdateStudy")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd'-'MM'-'yyyy}")]
        public string StartdateStudy { get; set; }

        [StringLength(100, ErrorMessage = "This street is invalid")]
        [DataType(DataType.Text)]
        [Display(Name = "Street")]
        public string AddressStreet { get; set; }

        [StringLength(5, ErrorMessage = "This number is invalid")]
        [DataType(DataType.Text)]
        [Display(Name = "Adress number")]
        public string AddressNumber { get; set; }

        [Required]
        [StringLength(8, ErrorMessage = "This number is invalid")]
        [DataType(DataType.Text)]
        [Display(Name = "Student number")]
        public string StudentNumber { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "This study type is invalid")]
        [DataType(DataType.Text)]
        [Display(Name = "Study type")]
        public string StudyType { get; set; }

        [StringLength(100, ErrorMessage = "This phone number is invalid")]
        [DataType(DataType.Text)]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [StringLength(7, ErrorMessage = "This postal code is invalid")]
        [DataType(DataType.Text)]
        [Display(Name = "Postal code")]
        public string AddressPostalCode { get; set; }

        [StringLength(100, ErrorMessage = "This city is invalid")]
        [Display(Name = "AddressCity")]
        public string AddressCity { get; set; }

    }
}
