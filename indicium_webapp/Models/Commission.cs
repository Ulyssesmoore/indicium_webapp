using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models
{
    [Table("Commissions")]
    public class Commission
    {
        public Commission()
        {
            Members = new HashSet<CommissionMember>();
        }

        public int CommissionID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<CommissionMember> Members { get; set; }
    }
}
