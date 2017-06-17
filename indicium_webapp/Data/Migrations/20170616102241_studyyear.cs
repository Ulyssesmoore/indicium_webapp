using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace indicium_webapp.Data.Migrations
{
    public partial class studyyear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartdateStudy",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "StartdateStudy",
                table: "AspNetUsers",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartdateStudy",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartdateStudy",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}