using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace indicium_webapp.Data.Migrations
{
    public partial class Guest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GuestID",
                table: "SignUp",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GuestID1",
                table: "SignUp",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Guest",
                columns: table => new
                {
                    GuestID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Email = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guest", x => x.GuestID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SignUp_GuestID1",
                table: "SignUp",
                column: "GuestID1");

            migrationBuilder.AddForeignKey(
                name: "FK_SignUp_Guest_GuestID1",
                table: "SignUp",
                column: "GuestID1",
                principalTable: "Guest",
                principalColumn: "GuestID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SignUp_Guest_GuestID1",
                table: "SignUp");

            migrationBuilder.DropTable(
                name: "Guest");

            migrationBuilder.DropIndex(
                name: "IX_SignUp_GuestID1",
                table: "SignUp");

            migrationBuilder.DropColumn(
                name: "GuestID",
                table: "SignUp");

            migrationBuilder.DropColumn(
                name: "GuestID1",
                table: "SignUp");
        }
    }
}
