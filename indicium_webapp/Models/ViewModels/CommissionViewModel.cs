using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace indicium_webapp.Models.ViewModels
{
    [Table("Commissions")]
    public class CommissionViewModel
    {
        public CommissionViewModel()
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
