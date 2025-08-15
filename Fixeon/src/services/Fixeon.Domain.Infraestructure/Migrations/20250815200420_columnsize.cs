using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fixeon.Domain.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class columnsize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "tickets",
                type: "varchar(3000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(2500)");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "interactions",
                type: "varchar(3000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "tickets",
                type: "varchar(2500)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(3000)");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "interactions",
                type: "varchar(500)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(3000)");
        }
    }
}
