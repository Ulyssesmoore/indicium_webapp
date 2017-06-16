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

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Name
        {
            get { return FirstName + " " + LastName; }
        }

        public string Email { get; set; }

        public virtual ICollection<SignUp> SignUps { get; set; }
    }
}
