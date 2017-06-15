using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace indicium_webapp.Data.Migrations
{
    public partial class commissionfinal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommissionID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Commissions",
                columns: table => new
                {
                    CommissionID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commissions", x => x.CommissionID);
                });

            migrationBuilder.CreateTable(
                name: "CommissionMembers",
                columns: table => new
                {
                    CommissionMemberID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ApplicationUserID = table.Column<string>(nullable: true),
                    CommissionID = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommissionMembers", x => x.CommissionMemberID);
                    table.ForeignKey(
                        name: "FK_CommissionMembers_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommissionMembers_Commissions_CommissionID",
                        column: x => x.CommissionID,
                        principalTable: "Commissions",
                        principalColumn: "CommissionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CommissionID",
                table: "AspNetUsers",
                column: "CommissionID");

            migrationBuilder.CreateIndex(
                name: "IX_CommissionMembers_ApplicationUserID",
                table: "CommissionMembers",
                column: "ApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_CommissionMembers_CommissionID",
                table: "CommissionMembers",
                column: "CommissionID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Commissions_CommissionID",
                table: "AspNetUsers",
                column: "CommissionID",
                principalTable: "Commissions",
                principalColumn: "CommissionID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Commissions_CommissionID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CommissionMembers");

            migrationBuilder.DropTable(
                name: "Commissions");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CommissionID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CommissionID",
                table: "AspNetUsers");
        }
    }
}
