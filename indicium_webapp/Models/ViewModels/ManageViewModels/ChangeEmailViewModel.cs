using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models.ViewModels.ManageViewModels
{
    public class ChangeEmailViewModel
    {
        [Required(ErrorMessage = "{0} is verplicht.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mailadres")]
        public string Email { get; set; }
    }
}
