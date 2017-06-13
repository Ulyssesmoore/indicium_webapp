using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace indicium_webapp.Models
{
    public enum CommisionMemberStatus
    {
        Interesse,
        Lid
    }

    [Table("CommissionMembers")]
    public class CommissionMember
    {
        public int CommissionMemberID { get; set; }

        public int CommissionID { get; set; }

        public virtual Commission Commission { get; set; }

        public string ApplicationUserID { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public CommisionMemberStatus Status { get; set; }
    }
}
