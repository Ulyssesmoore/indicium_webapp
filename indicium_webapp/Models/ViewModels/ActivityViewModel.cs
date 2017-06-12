using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models
{
    public class ActivityViewModel
    {
        public int ActivityID { get; set; }
        
        [Required]
        [StringLength(50)]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Omschrijving")]
        public string Description { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Startdatum is ongeldig.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Startdatum")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm}")]
        public String StartDateTime { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Einddatum is ongeldig.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Einddatum")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm}")]
        public String EndDateTime { get; set; }

        [Required]
        [Display(Name = "Inschrijving verplicht")]
        public bool NeedsSignUp { get; set; }

        [Display(Name = "Prijs")]
        public double Price { get; set; }

        [Required]
        public int ActivityTypeID { get; set; }
        
        [Display(Name = "Activiteittype")]
        public ActivityType ActivityType { get; set; }

        [Display(Name = "Inschrijvingen")]
        public virtual ICollection<SignUp> SignUps { get; set; }

    }
}
