using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models.ManageViewModels
{
    public class ChangeAddressInformationViewModel
    {
        [DataType(DataType.Text)]
        public string AddressCity { get; set; }

        [DataType(DataType.Text)]
        public string AddressPostalCode { get; set; }

        [DataType(DataType.Text)]
        public string AddressNumber { get; set; }

        [DataType(DataType.Text)]
        public string AddressStreet { get; set; }

        [DataType(DataType.Text)]
        public string AddressCountry { get; set; }
    }
}
