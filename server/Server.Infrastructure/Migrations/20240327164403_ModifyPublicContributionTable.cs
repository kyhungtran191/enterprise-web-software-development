using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyPublicContributionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContributionPublics_Contributions_Id",
                table: "ContributionPublics");

            migrationBuilder.AddColumn<Guid>(
                name: "AcademicYearId",
                table: "ContributionPublics",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "ContributionPublics",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "ContributionPublics",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEdited",
                table: "ContributionPublics",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FacultyId",
                table: "ContributionPublics",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "ContributionPublics",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCoordinatorComment",
                table: "ContributionPublics",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublicDate",
                table: "ContributionPublics",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "ContributionPublics",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ContributionPublics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmissionDate",
                table: "ContributionPublics",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ContributionPublics",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ContributionPublics",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcademicYearId",
                table: "ContributionPublics");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "ContributionPublics");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "ContributionPublics");

            migrationBuilder.DropColumn(
                name: "DateEdited",
                table: "ContributionPublics");

            migrationBuilder.DropColumn(
                name: "FacultyId",
                table: "ContributionPublics");

            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "ContributionPublics");

            migrationBuilder.DropColumn(
                name: "IsCoordinatorComment",
                table: "ContributionPublics");

            migrationBuilder.DropColumn(
                name: "PublicDate",
                table: "ContributionPublics");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "ContributionPublics");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ContributionPublics");

            migrationBuilder.DropColumn(
                name: "SubmissionDate",
                table: "ContributionPublics");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "ContributionPublics");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ContributionPublics");

            migrationBuilder.AddForeignKey(
                name: "FK_ContributionPublics_Contributions_Id",
                table: "ContributionPublics",
                column: "Id",
                principalTable: "Contributions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
