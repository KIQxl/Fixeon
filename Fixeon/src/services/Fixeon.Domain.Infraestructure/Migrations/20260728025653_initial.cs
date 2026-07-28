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
                name: "companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 200, nullable: false),
                    CNPJ = table.Column<string>(type: "varchar(100)", maxLength: 14, nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(100)", maxLength: 13, nullable: false),
                    Street = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Number = table.Column<string>(type: "varchar(100)", maxLength: 6, nullable: false),
                    Neighborhood = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: false),
                    State = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: false),
                    PostalCode = table.Column<string>(type: "varchar(100)", maxLength: 8, nullable: false),
                    Country = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "varchar(100)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Protocol = table.Column<string>(type: "varchar(100)", maxLength: 6, nullable: false),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "varchar(100)", maxLength: 3000, nullable: false),
                    Category = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: false),
                    Departament = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: false),
                    userId = table.Column<string>(type: "varchar(100)", maxLength: 36, nullable: false),
                    userEmail = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    OrganizationName = table.Column<string>(type: "varchar(100)", maxLength: 36, nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: true),
                    analystId = table.Column<string>(type: "varchar(100)", maxLength: 36, nullable: true),
                    analystEmail = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "varchar(100)", maxLength: 30, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Priority = table.Column<string>(type: "varchar(100)", maxLength: 20, nullable: false),
                    closedById = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    closedByName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstInteractionDeadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FirstInteractionAccomplished = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ResolutionDeadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ResolutionAccomplished = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tickets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "organizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: false),
                    CNPJ = table.Column<string>(type: "varchar(100)", maxLength: 14, nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(100)", maxLength: 13, nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Street = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Number = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    Neighborhood = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    State = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PostalCode = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    Country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "varchar(100)", maxLength: 1000, nullable: true),
                    ProfilePictureUrl = table.Column<string>(type: "varchar(100)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_organizations_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.Id);
                    table.ForeignKey(
                        name: "CompanyTags",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "interactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TicketId = table.Column<Guid>(type: "uuid", nullable: false),
                    Message = table.Column<string>(type: "varchar(100)", maxLength: 3000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    userId = table.Column<string>(type: "varchar(100)", maxLength: 36, nullable: false),
                    userEmail = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
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
                name: "categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category_Organization",
                        column: x => x.OrganizationId,
                        principalTable: "organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "departaments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departaments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departament_Organization",
                        column: x => x.OrganizationId,
                        principalTable: "organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "organizationsSLAs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    SLAInMinutes = table.Column<int>(type: "integer", nullable: false),
                    SLAPriority = table.Column<string>(type: "varchar(100)", maxLength: 30, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organizationsSLAs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_organizationsSLAs_organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketTags",
                columns: table => new
                {
                    TagId = table.Column<Guid>(type: "uuid", nullable: false),
                    TicketId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketTags", x => new { x.TagId, x.TicketId });
                    table.ForeignKey(
                        name: "FK_TicketTags_tags_TagId",
                        column: x => x.TagId,
                        principalTable: "tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketTags_tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "attachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Filename = table.Column<string>(type: "varchar(100)", maxLength: 200, nullable: false),
                    Extension = table.Column<string>(type: "varchar(100)", maxLength: 6, nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SenderId = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    TicketId = table.Column<Guid>(type: "uuid", nullable: true),
                    InteractionId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attachments", x => x.Id);
                    table.CheckConstraint("CK_Attachment_Ticket_Or_Interaction", "(\"TicketId\" IS NOT NULL AND \"InteractionId\" IS NULL) OR (\"TicketId\" IS NULL AND \"InteractionId\" IS NOT NULL)");
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
                name: "IX_categories_OrganizationId",
                table: "categories",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_companies_CNPJ",
                table: "companies",
                column: "CNPJ",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_departaments_OrganizationId",
                table: "departaments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_interactions_TicketId",
                table: "interactions",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_organizations_CompanyId",
                table: "organizations",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_organizationsSLAs_OrganizationId",
                table: "organizationsSLAs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_tags_CompanyId",
                table: "tags",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketTags_TicketId",
                table: "TicketTags",
                column: "TicketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "attachments");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "departaments");

            migrationBuilder.DropTable(
                name: "organizationsSLAs");

            migrationBuilder.DropTable(
                name: "TicketTags");

            migrationBuilder.DropTable(
                name: "interactions");

            migrationBuilder.DropTable(
                name: "organizations");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "tickets");

            migrationBuilder.DropTable(
                name: "companies");
        }
    }
}
