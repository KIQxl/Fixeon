using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fixeon.Domain.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class newAttributesForCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "companies",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "companies",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "companies",
                type: "varchar(6)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "companies",
                type: "varchar(13)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "companies",
                type: "varchar(8)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "companies",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "companies",
                type: "varchar(25)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "companies",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "State",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "companies");
        }
    }
}
