using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fixeon.Domain.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class renametickettagtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketTags_tags_TagId",
                table: "TicketTags");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketTags_tickets_TicketId",
                table: "TicketTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketTags",
                table: "TicketTags");

            migrationBuilder.RenameTable(
                name: "TicketTags",
                newName: "tickettags");

            migrationBuilder.RenameIndex(
                name: "IX_TicketTags_TicketId",
                table: "tickettags",
                newName: "IX_tickettags_TicketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tickettags",
                table: "tickettags",
                columns: new[] { "TagId", "TicketId" });

            migrationBuilder.AddForeignKey(
                name: "FK_tickettags_tags_TagId",
                table: "tickettags",
                column: "TagId",
                principalTable: "tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tickettags_tickets_TicketId",
                table: "tickettags",
                column: "TicketId",
                principalTable: "tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tickettags_tags_TagId",
                table: "tickettags");

            migrationBuilder.DropForeignKey(
                name: "FK_tickettags_tickets_TicketId",
                table: "tickettags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tickettags",
                table: "tickettags");

            migrationBuilder.RenameTable(
                name: "tickettags",
                newName: "TicketTags");

            migrationBuilder.RenameIndex(
                name: "IX_tickettags_TicketId",
                table: "TicketTags",
                newName: "IX_TicketTags_TicketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketTags",
                table: "TicketTags",
                columns: new[] { "TagId", "TicketId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TicketTags_tags_TagId",
                table: "TicketTags",
                column: "TagId",
                principalTable: "tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketTags_tickets_TicketId",
                table: "TicketTags",
                column: "TicketId",
                principalTable: "tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
