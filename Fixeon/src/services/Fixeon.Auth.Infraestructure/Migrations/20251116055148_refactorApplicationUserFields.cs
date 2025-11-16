using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fixeon.Auth.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class refactorApplicationUserFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                table: "AspNetUsers");
        }
    }
}
