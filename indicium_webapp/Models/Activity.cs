using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models
{
    public class Activity
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
        [Display(Name = "Start")]
        public DateTime StartDateTime { get; set; }

        [Required]
        [Display(Name = "Eind")]
        public DateTime EndDateTime { get; set; }

        [Required]
        [Display(Name = "Inschrijving Verplicht")]
        public bool NeedsSignUp { get; set; }

        [Display(Name = "Prijs")]
        public double Price { get; set; }
    }
}
