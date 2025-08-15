using Fixeon.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fixeon.Domain.Infraestructure.Mappings
{
    public class TicketMapping : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(t => t.Description)
                .IsRequired()
                .HasColumnType("varchar(3000)");

            builder.Property(t => t.Category)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(t => t.Departament)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(t => t.Priority)
                .IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(t => t.Status)
                .IsRequired()
                .HasColumnType("varchar(30)");

            builder.OwnsOne(t => t.CreatedByUser, user =>
            {
                user.Property(u => u.UserId)
                    .IsRequired()
                    .HasColumnType("varchar(36)")
                    .HasColumnName("userId");

                user.Property(u => u.UserEmail)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasColumnName("userEmail");

                user.Property(u => u.OrganizationId)
                    .HasColumnName("OrganizationId")
                    .HasColumnType("varchar(36)");

                user.Property(u => u.OrganizationName)
                    .HasColumnName("OrganizationName")
                    .HasColumnType("varchar(50)");
            });

            builder.OwnsOne(t => t.AssignedTo, analyst =>
            {
                analyst.Property(u => u.AnalystId)
                    .IsRequired()
                    .HasColumnType("varchar(36)")
                    .HasColumnName("analystId");

                analyst.Property(u => u.AnalystEmail)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasColumnName("analystEmail");
            });

            builder.Property(t => t.CreateAt)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(t => t.ModifiedAt)
                .HasColumnType("datetime");

            builder.Property(t => t.ResolvedAt)
                .HasColumnType("datetime");

            builder.OwnsOne(t => t.ClosedBy, analyst =>
            {
                analyst.Property(u => u.AnalystId)
                    .IsRequired()
                    .HasColumnType("varchar(36)")
                    .HasColumnName("closedById");

                analyst.Property(u => u.AnalystEmail)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasColumnName("closedByName");
            });

            builder.HasMany(t => t.Interactions)
                .WithOne(i => i.Ticket)
                .HasForeignKey(i => i.TicketId);

            builder.HasMany(t => t.Attachments)
                .WithOne(a => a.Ticket)
                .HasForeignKey(a => a.TicketId);
        }
    }
}
