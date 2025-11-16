using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fixeon.Domain.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class organizationNewFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "organizations",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "organizations",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Neighborhood",
                table: "organizations",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "organizations",
                type: "varchar(1000)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "organizations",
                type: "varchar(6)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "organizations",
                type: "varchar(13)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "organizations",
                type: "varchar(8)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "organizations",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "organizations",
                type: "varchar(25)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "organizations",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "organizations");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "organizations");

            migrationBuilder.DropColumn(
                name: "Neighborhood",
                table: "organizations");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "organizations");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "organizations");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "organizations");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "organizations");

            migrationBuilder.DropColumn(
                name: "State",
                table: "organizations");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "organizations");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "organizations");
        }
    }
}
