using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models.ViewModels.AccountViewModels
{
    public class ApplicationRoleViewModel
    {
        public string Id { get; set; }
        
        [Display(Name = "Naam")]
        public string Name { get; set; }
        
        [Display(Name = "Beschrijving")]
        public string Description { get; set; }

    }
}
