using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models
{
    [Table("Activities")]
    public class Activity
    {
        public Activity()
        {
            SignUps = new HashSet<SignUp>();
        }

        public int ActivityID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public bool NeedsSignUp { get; set; }

        public double Price { get; set; }

        public virtual ICollection<SignUp> SignUps { get; set; }

        public int ActivityTypeID { get; set; }

        // Property to help select related data
        public virtual ActivityType ActivityType { get; set; }
    }
}
