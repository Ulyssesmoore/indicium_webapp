using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models.ViewModels.AccountViewModels
{
    public class GuestViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "Voornaam mag maximaal 50 karakters.")]
        [Display(Name = "Voornaam")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Achternaam mag maximaal 100 karakters.")]
        [Display(Name = "Achternaam")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "E-mailAdres")] 
        public string Email { get; set; }

    }
}
