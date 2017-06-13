using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models.ManageViewModels
{
    public class ChangeEducationalInformationViewModel
    {
        [Required(ErrorMessage = "{0} is verplicht.")]
        [DataType(DataType.Text)]
        [Display(Name = "Studietype")]
        public StudyType StudyType { get; set; }

        [Required(ErrorMessage = "{0} is verplicht.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Begindatum studie")]
        public DateTime StartdateStudy { get; set; }
    }
}
