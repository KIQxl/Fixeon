using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fixeon.Domain.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class addimagesinteraction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstAttachment",
                table: "interactions",
                type: "varchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondAttachment",
                table: "interactions",
                type: "varchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThirdAttachment",
                table: "interactions",
                type: "varchar(250)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstAttachment",
                table: "interactions");

            migrationBuilder.DropColumn(
                name: "SecondAttachment",
                table: "interactions");

            migrationBuilder.DropColumn(
                name: "ThirdAttachment",
                table: "interactions");
        }
    }
}
