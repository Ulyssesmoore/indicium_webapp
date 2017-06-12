using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models
{
    [Table("Guests")]
    public class Guest
    {
        public Guest()
        {
            SignUps = new HashSet<SignUp>();
        }

        public int GuestID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Voornaam mag maximaal 50 karakters.")]
        [Display(Name = "Voornaam")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Achternaam mag maximaal 100 karakters.")]
        [Display(Name = "Achternaam")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email Adres")] 
        public string Email { get; set; }

        public virtual ICollection<SignUp> SignUps { get; set; }
    }
}
