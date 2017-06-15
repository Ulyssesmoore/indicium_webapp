using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models.ViewModels.ManageViewModels
{
    public class ChangePhoneNumberViewModel
    {
        [Required(ErrorMessage = "{0} is verplicht.")]
        [Phone]
        [Display(Name = "Telefoonnummer")]
        public string PhoneNumber { get; set; }
    }
}
