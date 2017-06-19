﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models.ViewModels.ManageViewModels
{
    public class ChangePersonalInformationViewModel
    {
        [Required(ErrorMessage = "{0} is verplicht.")]
        [DataType(DataType.Text)]
        [Display(Name = "Geslacht")]
        public Sex Sex { get; set; }
        
        [Required(ErrorMessage = "{0} is verplicht.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Geboortedatum")]
        public DateTime Birthday { get; set; }       
    }
}
