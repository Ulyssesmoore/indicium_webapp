using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using indicium_webapp.Models.AccountViewModels;

namespace indicium_webapp.Models
{
    public class SignUpViewModel
    {
        public int SignUpID { get; set; }

        public string Status { get; set; }

        public ActivityViewModel Activity { get; set; }

        public ApplicationUserViewModel ApplicationUser { get; set; }

        public GuestViewModel Guest { get; set; }

    }
}
