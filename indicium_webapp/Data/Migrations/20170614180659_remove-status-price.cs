using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace indicium_webapp.Data.Migrations
{
    public partial class removestatusprice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "SignUps");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Activities");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "SignUps",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Activities",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
