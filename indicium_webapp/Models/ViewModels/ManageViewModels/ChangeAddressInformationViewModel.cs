using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models.ViewModels.ManageViewModels
{
    public class ChangeAddressInformationViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string AddressCity { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string AddressPostalCode { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string AddressNumber { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string AddressStreet { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string AddressCountry { get; set; }
    }
}
