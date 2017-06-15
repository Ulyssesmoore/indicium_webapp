using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models.ViewModels.AccountViewModels
{
    public class VerifyCodeViewModel
    {
        [Required(ErrorMessage = "{0} is verplicht.")]
        public string Provider { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }

        [Display(Name = "Onthoud deze browser?")]
        public bool RememberBrowser { get; set; }

        [Display(Name = "Onthouden?")]
        public bool RememberMe { get; set; }
    }
}
