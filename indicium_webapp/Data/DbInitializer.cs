using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using indicium_webapp.Models;
using Microsoft.AspNetCore.Identity;

namespace indicium_webapp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Roles.Any())
            {
                return; // DB has been seeded already
            }

            var roles = new ApplicationRole[]
            {
                new ApplicationRole { Name = "Lid", NormalizedName = "LID", Description = "Een standaard lid"},
                new ApplicationRole { Name = "Bestuur", NormalizedName = "BESTUUR", Description = "Een bestuurslid"},
                new ApplicationRole { Name = "Secretaris", NormalizedName = "SECRETARIS", Description = "Een bestuurslid belast met secratariaat"},
            };          

            foreach (ApplicationRole r in roles)
            {
                context.Roles.Add(r);
            }

            context.SaveChanges();

            // DON'T USE THE SHIT DOWN BELOW

            //var users = new ApplicationUser[]
            //{
            //    new ApplicationUser {
            //        AddressCity = "Utrecht",
            //        AddressCountry = "Nederland",
            //        AddressStreet = "Een straat",
            //        AddressNumber = "42",
            //        AddressPostalCode = "1234AB",
            //        Birthday = new DateTime(1997, 5, 22),
            //        Email = "iemand@test.nl",
            //        NormalizedEmail = "IEMAND@TEST.NL",
            //        UserName = "iemand@test.nl",
            //        NormalizedUserName = "IEMAND@TEST.NL",
            //        FirstName = "Henk",
            //        LastName = "Tober",
            //        Sex = "M",
            //        StudentNumber = 123456,
            //        StartdateStudy = new DateTime(2016, 5, 22),
            //        StudyType = "SIE",
            //        PhoneNumber = "06123456",
            //    }
            //};

            //foreach (ApplicationUser u in users)
            //{
            //    u.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(u, "P@ssword12");
            //    context.Users.Add(u);
            //}

            //context.SaveChanges();
        }
    }
}
