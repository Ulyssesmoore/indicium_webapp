using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace indicium_webapp.Models
{
    [Table("SignUps")]
    public class SignUp
    {
        public int SignUpID { get; set; }

        public string Status { get; set; }

        public int ActivityID { get; set; }

        public virtual Activity Activity { get; set; }

        public string ApplicationUserID { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public int? GuestID { get; set; }

        public virtual Guest Guest { get; set; }

        // Validation to make sure there's at least a ApplicationUser or Guest coupled to the signup
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ApplicationUser == null || Guest == null)
            {
                yield return new ValidationResult("Er moet een gast of gebruiker zijn gekoppeld aan de inschrijving.");
            }
        }
    }

}
