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

        [Display(Name = "Naam")]
        public string Name { get; set; }

        [EmailAddress]
        [Display(Name = "E-mailadres")]
        public string Email { get; set; }

        [Display(Name = "Onderwerp")]
        public string Subject { get; set; }

        [Display(Name = "Bericht")]
        public string Message { get; set; }

    }
}
