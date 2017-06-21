using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models.ViewModels.HomeViewModels
{
    public class ContactViewModel
    {
        public int ContactID { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [EmailAddress]
        [Display(Name = "E-mailadres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [Display(Name = "Onderwerp")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [Display(Name = "Bericht")]
        public string Message { get; set; }

    }
}
