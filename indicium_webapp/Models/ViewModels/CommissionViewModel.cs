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
            CommissionMembers = new HashSet<CommissionMember>();
        }

        public int CommissionID { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(50, ErrorMessage = "{0} mag maximaal {1} karakter(s) zijn.")]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(200, ErrorMessage = "{0} mag maximaal {1} karakter(s) zijn.")]
        [Display(Name = "Beschrijving")]
        public string Description { get; set; }

        [Display(Name = "Leden")]
        public virtual ICollection<ApplicationUser> Members { get; set; }

        public virtual ICollection<CommissionMember> CommissionMembers { get; set; }
    }
}
