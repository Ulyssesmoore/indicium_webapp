﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models
{
    public class Activity
    {
        public Activity()
        {
            SignUps = new HashSet<SignUp>();
        }

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
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartDateTime { get; set; }

        [Required]
        [Display(Name = "Eind")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EndDateTime { get; set; }

        [Required]
        [Display(Name = "Inschrijving verplicht")]
        public bool NeedsSignUp { get; set; }

        [Display(Name = "Prijs")]
        public double Price { get; set; }

        [Required]
        [Display(Name = "Activiteit Type")]
        public int ActivityTypeID { get; set; }

        [Display(Name = "Inschrijvingen")]
        public virtual ICollection<SignUp> SignUps { get; set; }

        // Property to help select related data
        [Display(Name = "Activiteit Type")]
        public virtual ActivityType ActivityType { get; set; }
    }
}
