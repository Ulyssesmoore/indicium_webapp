using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace indicium_webapp.Data.Migrations
{
    public partial class signup_user_fk_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_AspNetUsers_ApplicationUserId",
                table: "Activity");

            migrationBuilder.DropIndex(
                name: "IX_Activity_ApplicationUserId",
                table: "Activity");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Activity");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserID",
                table: "SignUp",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_SignUp_ApplicationUserID",
                table: "SignUp",
                column: "ApplicationUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_SignUp_AspNetUsers_ApplicationUserID",
                table: "SignUp",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SignUp_AspNetUsers_ApplicationUserID",
                table: "SignUp");

            migrationBuilder.DropIndex(
                name: "IX_SignUp_ApplicationUserID",
                table: "SignUp");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserID",
                table: "SignUp",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Activity",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activity_ApplicationUserId",
                table: "Activity",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_AspNetUsers_ApplicationUserId",
                table: "Activity",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
