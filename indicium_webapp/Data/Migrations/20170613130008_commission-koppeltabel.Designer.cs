using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using indicium_webapp.Data;
using indicium_webapp.Models;

namespace indicium_webapp.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170613130008_commission-koppeltabel")]
    partial class commissionkoppeltabel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("indicium_webapp.Models.Activity", b =>
                {
                    b.Property<int>("ActivityID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActivityTypeID");

                    b.Property<string>("Description");

                    b.Property<DateTime>("EndDateTime");

                    b.Property<string>("Name");

                    b.Property<bool>("NeedsSignUp");

                    b.Property<double>("Price");

                    b.Property<DateTime>("StartDateTime");

                    b.HasKey("ActivityID");

                    b.HasIndex("ActivityTypeID");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("indicium_webapp.Models.ActivityType", b =>
                {
                    b.Property<int>("ActivityTypeID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BackgroundColor");

                    b.Property<string>("BorderColor");

                    b.Property<string>("Name");

                    b.Property<string>("TextColor");

                    b.HasKey("ActivityTypeID");

                    b.ToTable("ActivityTypes");
                });

            modelBuilder.Entity("indicium_webapp.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("AddressCity");

                    b.Property<string>("AddressCountry");

                    b.Property<string>("AddressNumber");

                    b.Property<string>("AddressPostalCode");

                    b.Property<string>("AddressStreet");

                    b.Property<DateTime>("Birthday");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("Iban");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<DateTime>("RegistrationDate");

                    b.Property<string>("SecurityStamp");

                    b.Property<int>("Sex");

                    b.Property<DateTime>("StartdateStudy");

                    b.Property<int>("Status");

                    b.Property<int>("StudentNumber");

                    b.Property<int>("StudyType");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("indicium_webapp.Models.Commission", b =>
                {
                    b.Property<int>("CommissionID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("CommissionID");

                    b.ToTable("Commissions");
                });

            modelBuilder.Entity("indicium_webapp.Models.CommissionMember", b =>
                {
                    b.Property<int>("CommissionMemberID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserID");

                    b.Property<int>("CommissionID");

                    b.Property<int>("Status");

                    b.HasKey("CommissionMemberID");

                    b.HasIndex("ApplicationUserID");

                    b.HasIndex("CommissionID");

                    b.ToTable("CommissionMembers");
                });

            modelBuilder.Entity("indicium_webapp.Models.Guest", b =>
                {
                    b.Property<int>("GuestID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.HasKey("GuestID");

                    b.ToTable("Guests");
                });

            modelBuilder.Entity("indicium_webapp.Models.SignUp", b =>
                {
                    b.Property<int>("SignUpID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActivityID");

                    b.Property<string>("ApplicationUserID");

                    b.Property<int?>("GuestID");

                    b.Property<string>("Status");

                    b.HasKey("SignUpID");

                    b.HasIndex("ActivityID");

                    b.HasIndex("ApplicationUserID");

                    b.HasIndex("GuestID");

                    b.ToTable("SignUps");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityRole");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("indicium_webapp.Models.ApplicationRole", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole");

                    b.Property<string>("Description");

                    b.ToTable("ApplicationRole");

                    b.HasDiscriminator().HasValue("ApplicationRole");
                });

            modelBuilder.Entity("indicium_webapp.Models.Activity", b =>
                {
                    b.HasOne("indicium_webapp.Models.ActivityType", "ActivityType")
                        .WithMany()
                        .HasForeignKey("ActivityTypeID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("indicium_webapp.Models.CommissionMember", b =>
                {
                    b.HasOne("indicium_webapp.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("Commissions")
                        .HasForeignKey("ApplicationUserID");

                    b.HasOne("indicium_webapp.Models.Commission", "Commission")
                        .WithMany("Members")
                        .HasForeignKey("CommissionID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("indicium_webapp.Models.SignUp", b =>
                {
                    b.HasOne("indicium_webapp.Models.Activity", "Activity")
                        .WithMany("SignUps")
                        .HasForeignKey("ActivityID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("indicium_webapp.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("SignUps")
                        .HasForeignKey("ApplicationUserID");

                    b.HasOne("indicium_webapp.Models.Guest", "Guest")
                        .WithMany("SignUps")
                        .HasForeignKey("GuestID");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("indicium_webapp.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("indicium_webapp.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("indicium_webapp.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
