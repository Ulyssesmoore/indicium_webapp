using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace indicium_webapp.Data.Migrations
{
    public partial class activitytype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivityTypeID",
                table: "Activity",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ActivityType",
                columns: table => new
                {
                    ActivityTypeID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    BackgroundColor = table.Column<string>(nullable: true),
                    LineColor = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    TextColor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityType", x => x.ActivityTypeID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activity_ActivityTypeID",
                table: "Activity",
                column: "ActivityTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_ActivityType_ActivityTypeID",
                table: "Activity",
                column: "ActivityTypeID",
                principalTable: "ActivityType",
                principalColumn: "ActivityTypeID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_ActivityType_ActivityTypeID",
                table: "Activity");

            migrationBuilder.DropTable(
                name: "ActivityType");

            migrationBuilder.DropIndex(
                name: "IX_Activity_ActivityTypeID",
                table: "Activity");

            migrationBuilder.DropColumn(
                name: "ActivityTypeID",
                table: "Activity");
        }
    }
}
