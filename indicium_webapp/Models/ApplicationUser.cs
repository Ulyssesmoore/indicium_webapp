using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace indicium_webapp.Models
{
    public enum Sex
    {
        Man,
        Vrouw
    }

    public enum StudyType
    {
        Propedeuse,
        SIE,
        TI,
        SNE,
        BIM
    }
    
    public enum Status
    {
        Nieuw,
        Afgekeurd,
        Lid,
        Uitgeschreven,
        Alumni
    }

    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            SignUps = new HashSet<SignUp>();
            Commissions = new HashSet<CommissionMember>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Sex Sex { get; set; }

        public DateTime Birthday { get; set; }

        public string AddressStreet { get; set; }

        public string AddressNumber { get; set; }

        public string AddressPostalCode { get; set; }

        public string AddressCity { get; set; }

        public string AddressCountry { get; set; }

        public string Iban { get; set; }

        public int StudentNumber { get; set; }

        public DateTime StartdateStudy { get; set; }

        public StudyType StudyType { get; set; }

        public DateTime RegistrationDate { get; set; }

        public Status Status { get; set; }

        public virtual ICollection<SignUp> SignUps { get; set; }

        public virtual ICollection<CommissionMember> Commissions { get; set; }
    }
}
