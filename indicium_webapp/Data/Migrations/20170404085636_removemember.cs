using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace indicium_webapp.Data.Migrations
{
    public partial class removemember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Members");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    MemberID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddressCity = table.Column<string>(nullable: false),
                    AddressCountry = table.Column<string>(nullable: false),
                    AddressNumber = table.Column<string>(nullable: false),
                    AddressPostalCode = table.Column<string>(nullable: false),
                    AddressStreet = table.Column<string>(nullable: false),
                    Birthday = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    Iban = table.Column<string>(nullable: true),
                    IsActive = table.Column<int>(nullable: false),
                    LastName = table.Column<string>(maxLength: 100, nullable: false),
                    MemberAccountId = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: false),
                    RegistrationDate = table.Column<DateTime>(nullable: false),
                    Role = table.Column<int>(nullable: false),
                    Sex = table.Column<int>(nullable: false),
                    StartdateStudy = table.Column<DateTime>(nullable: false),
                    StudentNumber = table.Column<int>(nullable: false),
                    StudyType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.MemberID);
                    table.ForeignKey(
                        name: "FK_Members_AspNetUsers_MemberAccountId",
                        column: x => x.MemberAccountId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_MemberAccountId",
                table: "Members",
                column: "MemberAccountId");
        }
    }
}
