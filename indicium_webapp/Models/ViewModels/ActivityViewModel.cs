using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models.ViewModels
{
    public class ActivityViewModel
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
        public String StartDateTime { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(100, ErrorMessage = "{0} mag maximaal {1} karakter(s) zijn.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Einddatum")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm}")]
        public String EndDateTime { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [Display(Name = "Inschrijving verplicht")]
        public bool NeedsSignUp { get; set; }

        [Display(Name = "Prijs")]
        public double Price { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [Display(Name = "Activiteittype")]
        public int ActivityTypeID { get; set; }
        
        [Display(Name = "Activiteittype")]
        public virtual ActivityType ActivityType { get; set; }

        [Display(Name = "Inschrijvingen")]
        public virtual ICollection<SignUp> SignUps { get; set; }

    }
}
