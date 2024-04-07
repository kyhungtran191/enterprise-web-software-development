using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCompositeKeyInDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ContributionPublicComments",
                table: "ContributionPublicComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContributionComments",
                table: "ContributionComments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateEdited",
                table: "ContributionPublicComments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ContributionPublicComments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateEdited",
                table: "ContributionComments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ContributionComments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContributionPublicComments",
                table: "ContributionPublicComments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContributionComments",
                table: "ContributionComments",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ContributionPublicComments",
                table: "ContributionPublicComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContributionComments",
                table: "ContributionComments");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ContributionPublicComments");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ContributionComments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateEdited",
                table: "ContributionPublicComments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateEdited",
                table: "ContributionComments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContributionPublicComments",
                table: "ContributionPublicComments",
                columns: new[] { "ContributionId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContributionComments",
                table: "ContributionComments",
                columns: new[] { "ContributionId", "UserId" });
        }
    }
}
