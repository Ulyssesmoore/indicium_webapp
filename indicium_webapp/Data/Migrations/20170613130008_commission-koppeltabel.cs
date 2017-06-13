using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace indicium_webapp.Data.Migrations
{
    public partial class commissionkoppeltabel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Commissions_CommissionID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CommissionID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CommissionID",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommissionID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CommissionID",
                table: "AspNetUsers",
                column: "CommissionID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Commissions_CommissionID",
                table: "AspNetUsers",
                column: "CommissionID",
                principalTable: "Commissions",
                principalColumn: "CommissionID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
