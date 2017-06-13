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
        public static async Task Initialize(ApplicationDbContext context)
        {
            var roleStore = new RoleStore<ApplicationRole>(context);
            var roleManager = new RoleManager<ApplicationRole>(roleStore, null, null, null, null, null);

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore, null, null, null, null, null, null, null, null);

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

            // DIT WERKT NIET

            //if (!context.ApplicationUser.Any(u => u.FirstName == "Admin"))
            //{
            //    var user = new ApplicationUser
            //    {
            //        StudentNumber = 0000000,
            //        FirstName = "Admin",
            //        LastName = "Admin",
            //        PhoneNumber = "123456789",
            //        UserName = "admin@admin.com",
            //        Email = "admin@admin.com",
            //        Sex = Sex.Man,
            //        Birthday = DateTime.ParseExact("01-01-1997", "dd-MM-yyyy", new CultureInfo("nl-NL")),
            //        AddressCity = "Admindam",
            //        AddressStreet = "Adminstraat",
            //        AddressNumber = "1",
            //        AddressPostalCode = "1234Ab",
            //        AddressCountry = "Adminland",
            //        StartdateStudy = DateTime.ParseExact("01-01-2015", "dd-MM-yyyy", new CultureInfo("nl-NL")),
            //        RegistrationDate = DateTime.Today,
            //        StudyType = StudyType.SIE,
            //        Status = Status.Lid
            //    };

            //    await userManager.CreateAsync(user, "admin");

            //    foreach (var role in roles)
            //    {
            //        await userManager.AddToRoleAsync(user, role.Name);
            //    }

            //    context.SaveChanges();
            //}
        }
    }
}
