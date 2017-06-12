using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace indicium_webapp.Data.Migrations
{
    public partial class fixed_signups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SignUp_Guest_GuestID",
                table: "SignUp");

            migrationBuilder.AlterColumn<int>(
                name: "GuestID",
                table: "SignUp",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_SignUp_Guest_GuestID",
                table: "SignUp",
                column: "GuestID",
                principalTable: "Guest",
                principalColumn: "GuestID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SignUp_Guest_GuestID",
                table: "SignUp");

            migrationBuilder.AlterColumn<int>(
                name: "GuestID",
                table: "SignUp",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SignUp_Guest_GuestID",
                table: "SignUp",
                column: "GuestID",
                principalTable: "Guest",
                principalColumn: "GuestID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
