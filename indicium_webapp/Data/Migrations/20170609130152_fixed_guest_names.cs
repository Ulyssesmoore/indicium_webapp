using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace indicium_webapp.Data.Migrations
{
    public partial class fixed_guest_names : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SignUp_Guest_GuestID1",
                table: "SignUp");

            migrationBuilder.DropIndex(
                name: "IX_SignUp_GuestID1",
                table: "SignUp");

            migrationBuilder.DropColumn(
                name: "GuestID",
                table: "SignUp");
            
            migrationBuilder.DropColumn(
                name: "GuestID1",
                table: "SignUp");
            
            migrationBuilder.AddColumn<int>(
                name: "GuestID",
                table: "SignUp",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_SignUp_GuestID",
                table: "SignUp",
                column: "GuestID");

            migrationBuilder.AddForeignKey(
                name: "FK_SignUp_Guest_GuestID",
                table: "SignUp",
                column: "GuestID",
                principalTable: "Guest",
                principalColumn: "GuestID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SignUp_Guest_GuestID",
                table: "SignUp");

            migrationBuilder.DropIndex(
                name: "IX_SignUp_GuestID",
                table: "SignUp");

            migrationBuilder.AlterColumn<string>(
                name: "GuestID",
                table: "SignUp",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "GuestID1",
                table: "SignUp",
                nullable: true);

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
    }
}
