using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using indicium_webapp.Models.InterfaceItemModels;

namespace indicium_webapp.Models.ViewModels.ManageViewModels
{
    public class IndexViewModel
    {
        [Display(Name = "Voornaam")]
        public string FirstName { get; set; }

        [Display(Name = "Achternaam")]
        public string LastName { get; set; }
        
        [Display(Name = "Naam")]
        public string Name {
            get { return FirstName + " " + LastName; }
        }
        
        [Display(Name = "E-mailadres")]
        public string Email { get; set; }

        [Display(Name = "Geslacht")]
        public Sex Sex { get; set; }

        [Display(Name = "Geboortedatum")]
        public string Birthday { get; set; }

        [Display(Name = "Straat")]
        public string AddressStreet { get; set; }

        [Display(Name = "Huisnummer")]
        public string AddressNumber { get; set; }
        
        [Display(Name = "Postcode")]
        public string AddressPostalCode { get; set; }

        [Display(Name = "Woonplaats")]
        public string AddressCity { get; set; }

        [Display(Name = "Land")]
        public string AddressCountry { get; set; }

        [Display(Name = "Studentnummer")]
        public string StudentNumber { get; set; }

        [Display(Name = "Beginjaar studie")]
        public int StartdateStudy { get; set; }

        [Display(Name = "Studietype")]
        public StudyType StudyType { get; set; }

        [Display(Name = "Telefoonnummer")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Rollen")]
        public IEnumerable<CheckBoxListItem> Roles { get; set; }

    }
}
