using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models.ViewModels.ManageViewModels
{
    public class ChangeEducationalInformationViewModel
    {
        [Display(Name = "Studentnummer")]
        public string StudentNumber { get; set; }
        
        [Required(ErrorMessage = "{0} is verplicht.")]
        [DataType(DataType.Text)]
        [Display(Name = "Studietype")]
        public StudyType StudyType { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [Display(Name = "Beginjaar studie")]
        public int StartdateStudy { get; set; }
    }
}
