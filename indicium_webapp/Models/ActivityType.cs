using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models
{
    [Table("ActivityTypes")]
    public class ActivityType
    {
        public ActivityType()
        {
            //Activities = new HashSet<Activity>();
        }

        public int ActivityTypeID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Dit is geen correcte HEX kleur")] // Hex color
        [Display(Name = "Achtergrondkleur")]
        public string BackgroundColor { get; set; }

        [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Dit is geen correcte HEX kleur")] // Hex color
        [Display(Name = "Lijnkleur")]
        public string BorderColor { get; set; }

        [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Dit is geen correcte HEX kleur")] // Hex color
        [Display(Name = "Tekstkleur")]
        public string TextColor { get; set; }

        //[Display(Name = "Activiteiten")]
        //public virtual ICollection<Activity> Activities { get; set; }
    }
}
