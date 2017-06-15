using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using indicium_webapp.Models.ViewModels.AccountViewModels;
using indicium_webapp.Models.ViewModels;
using indicium_webapp.Models;

namespace indicium_webapp.Models.ViewModels
{
    public class SignUpViewModel
    {
        public int SignUpID { get; set; }

        public ActivityViewModel Activity { get; set; }

        public ApplicationUserViewModel ApplicationUser { get; set; }

        public GuestViewModel Guest { get; set; }

    }
}
