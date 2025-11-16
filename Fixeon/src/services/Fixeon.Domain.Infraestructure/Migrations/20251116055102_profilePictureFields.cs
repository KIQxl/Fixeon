using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fixeon.Domain.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class profilePictureFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                table: "organizations",
                type: "varchar(1000)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                table: "companies",
                type: "varchar(1000)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                table: "organizations");

            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                table: "companies");
        }
    }
}
