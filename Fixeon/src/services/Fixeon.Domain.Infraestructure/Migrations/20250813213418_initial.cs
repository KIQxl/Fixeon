using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fixeon.Domain.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "varchar(100)", nullable: false),
                    Description = table.Column<string>(type: "varchar(2500)", nullable: false),
                    Category = table.Column<string>(type: "varchar(50)", nullable: false),
                    Departament = table.Column<string>(type: "varchar(50)", nullable: false),
                    userId = table.Column<string>(type: "varchar(36)", nullable: false),
                    userEmail = table.Column<string>(type: "varchar(100)", nullable: false),
                    OrganizationName = table.Column<string>(type: "varchar(50)", nullable: true),
                    OrganizationId = table.Column<string>(type: "varchar(36)", nullable: true),
                    analystId = table.Column<string>(type: "varchar(36)", nullable: true),
                    analystName = table.Column<string>(type: "varchar(100)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<string>(type: "varchar(30)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    Priority = table.Column<string>(type: "varchar(20)", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tickets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "interactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "varchar(500)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    userId = table.Column<string>(type: "varchar(36)", nullable: false),
                    userEmail = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_interactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_interactions_tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "attachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Filename = table.Column<string>(type: "varchar(200)", nullable: false),
                    Extension = table.Column<string>(type: "varchar(6)", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    SenderId = table.Column<string>(type: "varchar(36)", nullable: false),
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attachments", x => x.Id);
                    table.CheckConstraint("CK_Attachment_Ticket_Or_Interaction", "(TicketId IS NOT NULL AND InteractionId IS NULL) OR (TicketId IS NULL AND InteractionId IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_attachments_interactions_InteractionId",
                        column: x => x.InteractionId,
                        principalTable: "interactions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_attachments_tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "tickets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_attachments_InteractionId",
                table: "attachments",
                column: "InteractionId");

            migrationBuilder.CreateIndex(
                name: "IX_attachments_TicketId",
                table: "attachments",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_interactions_TicketId",
                table: "interactions",
                column: "TicketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "attachments");

            migrationBuilder.DropTable(
                name: "interactions");

            migrationBuilder.DropTable(
                name: "tickets");
        }
    }
}
