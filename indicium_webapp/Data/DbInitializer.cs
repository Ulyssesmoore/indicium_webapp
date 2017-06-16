using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using indicium_webapp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace indicium_webapp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            var roleStore = new RoleStore<ApplicationRole>(context);
            var roleManager = new RoleManager<ApplicationRole>(roleStore, null, null, null, null, null);

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore, null, null, null, null, null, null, null, null);

            context.Database.EnsureCreated();

            if (!context.Roles.Any())
            {
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
            }

            /*
            var email = "usergenerated@test.test";
            if (!context.ApplicationUser.Any(x => x.Email == email))
            {
                var user = new ApplicationUser
                {
                    StudentNumber = 1234567,
                    FirstName = "Voornaam",
                    LastName = "Achternaam",
                    PhoneNumber = "123456789",
                    UserName = email,
                    Email = email,
                    Sex = (Sex)0,
                    Birthday = DateTime.ParseExact("14-06-2017", "dd-MM-yyyy", new CultureInfo("nl-NL")),
                    AddressCity = "Stad",
                    AddressStreet = "Straat",
                    AddressNumber = "1",
                    AddressPostalCode = "1234AB",
                    AddressCountry = "Nederland",
                    StartdateStudy = DateTime.ParseExact("14-06-2017", "dd-MM-yyyy", new CultureInfo("nl-NL")),
                    RegistrationDate = DateTime.Today,
                    StudyType = (StudyType)1,
                    Status = (Status)2
                };

                userManager.AddPasswordAsync(user, "Wachtwoord");
                userManager.CreateAsync(user);
                userManager.AddToRoleAsync(user, "Secretaris");
            }
            */
        }
    }
}
