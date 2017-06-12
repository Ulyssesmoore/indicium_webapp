using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models
{
    [Table("Commissions")]
    public class Commission
    {
        public Commission()
        {
            Members = new HashSet<ApplicationUser>();
        }

        public int CommissionID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Beschrijving")]
        public string Description { get; set; }

        [Display(Name = "Leden")]
        public virtual ICollection<ApplicationUser> Members { get; set; }
    }
}
