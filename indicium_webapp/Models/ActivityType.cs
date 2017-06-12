using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models
{
    [Table("ActivityTypes")]
    public class ActivityType
    {
        public int ActivityTypeID { get; set; }

        public string Name { get; set; }

        public string BackgroundColor { get; set; }

        public string BorderColor { get; set; }

        public string TextColor { get; set; }
    }
}
