using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace indicium_webapp.Data.Migrations
{
    public partial class UserForiegnKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
