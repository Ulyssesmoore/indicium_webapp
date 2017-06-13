using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models
{
    public class ActivityTypeViewModel
    {
        public int ActivityTypeID { get; set; }
        
        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(50, ErrorMessage = "{0} mag maximaal {1} karakter(s) zijn.")]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Dit is geen correcte HEX kleur")]
        [Display(Name = "Achtergrondkleur")]
        public string BackgroundColor { get; set; }

        [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Dit is geen correcte HEX kleur")]
        [Display(Name = "Lijnkleur")]
        public string BorderColor { get; set; }

        [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Dit is geen correcte HEX kleur")]
        [Display(Name = "Tekstkleur")]
        public string TextColor { get; set; }

    }
}
