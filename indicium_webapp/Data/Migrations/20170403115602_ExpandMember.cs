using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace indicium_webapp.Data.Migrations
{
    public partial class ExpandMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Studentnumber",
                table: "Members",
                newName: "StudentNumber");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Members",
                newName: "MemberID");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Members",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Members",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressCity",
                table: "Members",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AddressCountry",
                table: "Members",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AddressNumber",
                table: "Members",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AddressPostalCode",
                table: "Members",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AddressStreet",
                table: "Members",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Birthday",
                table: "Members",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Members",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Iban",
                table: "Members",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IsActive",
                table: "Members",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MemberAccountId",
                table: "Members",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Members",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                table: "Members",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Members",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sex",
                table: "Members",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartdateStudy",
                table: "Members",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "StudyType",
                table: "Members",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_MemberAccountId",
                table: "Members",
                column: "MemberAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_AspNetUsers_MemberAccountId",
                table: "Members",
                column: "MemberAccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_AspNetUsers_MemberAccountId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_MemberAccountId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "AddressCity",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "AddressCountry",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "AddressNumber",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "AddressPostalCode",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "AddressStreet",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Iban",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "MemberAccountId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Sex",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "StartdateStudy",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "StudyType",
                table: "Members");

            migrationBuilder.RenameColumn(
                name: "StudentNumber",
                table: "Members",
                newName: "Studentnumber");

            migrationBuilder.RenameColumn(
                name: "MemberID",
                table: "Members",
                newName: "ID");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Members",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Members",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);
        }
    }
}
