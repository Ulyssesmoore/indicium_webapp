using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models
{
    public class SignUp
    {
        public int SignUpID { get; set; }

        [Required]
        public int ActivityID { get; set; }

        public int ApplicationUserID { get; set; }

        [Required]
        public string Status { get; set; }
    }
}
