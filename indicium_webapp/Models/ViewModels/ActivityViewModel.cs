using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models.ViewModels
{
    public class ActivityViewModel : IValidatableObject
    {
        public int ActivityID { get; set; }
        
        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(50, ErrorMessage = "{0} mag maximaal {1} karakter(s) zijn.")]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [Display(Name = "Omschrijving")]
        public string Description { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(100, ErrorMessage = "{0} mag maximaal {1} karakter(s) zijn.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Startdatum")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm}")]
        public string StartDateTime { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(100, ErrorMessage = "{0} mag maximaal {1} karakter(s) zijn.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Einddatum")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm}")]
        public string EndDateTime { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [Display(Name = "Inschrijving verplicht")]
        public bool NeedsSignUp { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [Display(Name = "Activiteittype")]
        public string ActivityTypeID { get; set; }

        [Display(Name = "Activiteittype")]
        public ActivityType ActivityType { get; set; }

        [Display(Name = "Inschrijvingen")]
        public virtual ICollection<SignUp> SignUps { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateTime.ParseExact(EndDateTime, "dd-MM-yyyy HH:mm", new CultureInfo("nl-NL")) < DateTime.ParseExact(StartDateTime, "dd-MM-yyyy HH:mm", new CultureInfo("nl-NL")))
            {
                yield return
                  new ValidationResult(errorMessage: "Einddatum moet na de startdatum liggen",
                                       memberNames: new[] { "EndDateTime" });
            }
        }

    }
}
