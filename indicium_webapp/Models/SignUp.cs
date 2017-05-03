using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace indicium_webapp.Models
{
    public class SignUp
    {
        public int SignUpID { get; set; }

        [Required]
        public int ActivityID { get; set; }

        public string ApplicationUserID { get; set; }

        [Required]
        public string Status { get; set; }
    }
}
