using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace indicium_webapp.Data.Migrations
{
    public partial class pluraltablenames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_ActivityType_ActivityTypeID",
                table: "Activity");

            migrationBuilder.DropForeignKey(
                name: "FK_SignUp_Activity_ActivityID",
                table: "SignUp");

            migrationBuilder.DropForeignKey(
                name: "FK_SignUp_AspNetUsers_ApplicationUserID",
                table: "SignUp");

            migrationBuilder.DropForeignKey(
                name: "FK_SignUp_Guest_GuestID",
                table: "SignUp");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SignUp",
                table: "SignUp");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Guest",
                table: "Guest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActivityType",
                table: "ActivityType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activity",
                table: "Activity");

            migrationBuilder.RenameTable(
                name: "SignUp",
                newName: "SignUps");

            migrationBuilder.RenameTable(
                name: "Guest",
                newName: "Guests");

            migrationBuilder.RenameTable(
                name: "ActivityType",
                newName: "ActivityTypes");

            migrationBuilder.RenameTable(
                name: "Activity",
                newName: "Activities");

            migrationBuilder.RenameIndex(
                name: "IX_SignUp_GuestID",
                table: "SignUps",
                newName: "IX_SignUps_GuestID");

            migrationBuilder.RenameIndex(
                name: "IX_SignUp_ApplicationUserID",
                table: "SignUps",
                newName: "IX_SignUps_ApplicationUserID");

            migrationBuilder.RenameIndex(
                name: "IX_SignUp_ActivityID",
                table: "SignUps",
                newName: "IX_SignUps_ActivityID");

            migrationBuilder.RenameIndex(
                name: "IX_Activity_ActivityTypeID",
                table: "Activities",
                newName: "IX_Activities_ActivityTypeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SignUps",
                table: "SignUps",
                column: "SignUpID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Guests",
                table: "Guests",
                column: "GuestID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActivityTypes",
                table: "ActivityTypes",
                column: "ActivityTypeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activities",
                table: "Activities",
                column: "ActivityID");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_ActivityTypes_ActivityTypeID",
                table: "Activities",
                column: "ActivityTypeID",
                principalTable: "ActivityTypes",
                principalColumn: "ActivityTypeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SignUps_Activities_ActivityID",
                table: "SignUps",
                column: "ActivityID",
                principalTable: "Activities",
                principalColumn: "ActivityID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SignUps_AspNetUsers_ApplicationUserID",
                table: "SignUps",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SignUps_Guests_GuestID",
                table: "SignUps",
                column: "GuestID",
                principalTable: "Guests",
                principalColumn: "GuestID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_ActivityTypes_ActivityTypeID",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_SignUps_Activities_ActivityID",
                table: "SignUps");

            migrationBuilder.DropForeignKey(
                name: "FK_SignUps_AspNetUsers_ApplicationUserID",
                table: "SignUps");

            migrationBuilder.DropForeignKey(
                name: "FK_SignUps_Guests_GuestID",
                table: "SignUps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SignUps",
                table: "SignUps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Guests",
                table: "Guests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActivityTypes",
                table: "ActivityTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activities",
                table: "Activities");

            migrationBuilder.RenameTable(
                name: "SignUps",
                newName: "SignUp");

            migrationBuilder.RenameTable(
                name: "Guests",
                newName: "Guest");

            migrationBuilder.RenameTable(
                name: "ActivityTypes",
                newName: "ActivityType");

            migrationBuilder.RenameTable(
                name: "Activities",
                newName: "Activity");

            migrationBuilder.RenameIndex(
                name: "IX_SignUps_GuestID",
                table: "SignUp",
                newName: "IX_SignUp_GuestID");

            migrationBuilder.RenameIndex(
                name: "IX_SignUps_ApplicationUserID",
                table: "SignUp",
                newName: "IX_SignUp_ApplicationUserID");

            migrationBuilder.RenameIndex(
                name: "IX_SignUps_ActivityID",
                table: "SignUp",
                newName: "IX_SignUp_ActivityID");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_ActivityTypeID",
                table: "Activity",
                newName: "IX_Activity_ActivityTypeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SignUp",
                table: "SignUp",
                column: "SignUpID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Guest",
                table: "Guest",
                column: "GuestID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActivityType",
                table: "ActivityType",
                column: "ActivityTypeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activity",
                table: "Activity",
                column: "ActivityID");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_ActivityType_ActivityTypeID",
                table: "Activity",
                column: "ActivityTypeID",
                principalTable: "ActivityType",
                principalColumn: "ActivityTypeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SignUp_Activity_ActivityID",
                table: "SignUp",
                column: "ActivityID",
                principalTable: "Activity",
                principalColumn: "ActivityID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SignUp_AspNetUsers_ApplicationUserID",
                table: "SignUp",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SignUp_Guest_GuestID",
                table: "SignUp",
                column: "GuestID",
                principalTable: "Guest",
                principalColumn: "GuestID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
