using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace indicium_webapp.Models.ViewModels.ManageViewModels
{
    public class IndexViewModel
    {
        public string Email { get; set; }

        public string AddressCity { get; set; }

        public string AddressPostalCode { get; set; }

        public string AddressNumber { get; set; }

        public string AddressStreet { get; set; }

        public string AddressCountry { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime StartdateStudy { get; set; }

        public Sex Sex { get; set; }

        public DateTime Birthday { get; set; }

        public string Iban { get; set; }

        public StudyType StudyType { get; set; }

        //public bool BrowserRemembered { get; set; }

        public string Name { get; set; }
    }
}
