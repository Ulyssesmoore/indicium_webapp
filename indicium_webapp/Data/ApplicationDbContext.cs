﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using indicium_webapp.Models;
using indicium_webapp.Data;

namespace indicium_webapp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<indicium_webapp.Models.ApplicationUser> ApplicationUser { get; set; }
        public DbSet<indicium_webapp.Models.ApplicationRole> ApplicationRole { get; set; }
        public DbSet<indicium_webapp.Models.Activity> Activity { get; set; }
        public DbSet<indicium_webapp.Models.ActivityType> ActivityType { get; set; }
        public DbSet<indicium_webapp.Models.SignUp> SignUp { get; set; }
        public DbSet<indicium_webapp.Models.Commission> Commission { get; set; }
        public DbSet<indicium_webapp.Models.CommissionMember> CommissionMember { get; set; }
    }
}
