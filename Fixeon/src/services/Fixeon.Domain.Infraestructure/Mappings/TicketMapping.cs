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
                .HasMaxLength(100);

            builder.Property(t => t.Protocol)
                .IsRequired()
                .HasMaxLength(6);

            builder.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(3000);

            builder.Property(t => t.Category)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.Departament)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.Priority)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(t => t.Status)
                .IsRequired()
                .HasMaxLength(30);

            builder.OwnsOne(t => t.CreatedByUser, user =>
            {
                user.Property(u => u.UserId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .HasColumnName("userId");

                user.Property(u => u.UserEmail)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("userEmail");

                user.Property(u => u.OrganizationId)
                    .HasColumnName("OrganizationId")
                    .HasMaxLength(36);

                user.Property(u => u.OrganizationName)
                    .HasColumnName("OrganizationName")
                    .HasMaxLength(36);
            });

            builder.OwnsOne(t => t.AssignedTo, analyst =>
            {
                analyst.Property(u => u.AnalystId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .HasColumnName("analystId");

                analyst.Property(u => u.AnalystEmail)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("analystEmail");
            });

            builder.Property(t => t.CreateAt)
                .IsRequired();

            builder.Property(t => t.ModifiedAt);

            builder.Property(t => t.ResolvedAt);

            builder.OwnsOne(t => t.ClosedBy, analyst =>
            {
                analyst.Property(u => u.AnalystId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .HasColumnName("closedById");

                analyst.Property(u => u.AnalystEmail)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("closedByName");
            });

            builder.OwnsOne(t => t.SLAInfo, SLA =>
            {
                SLA.OwnsOne(s => s.FirstInteraction, FI =>
                {
                    FI.Property(x => x.Deadline)
                    .HasColumnName("FirstInteractionDeadline");

                    FI.Property(x => x.Accomplished)
                    .HasColumnName("FirstInteractionAccomplished");

                });

                SLA.OwnsOne(s => s.Resolution, R =>
                {
                    R.Property(x => x.Deadline)
                    .HasColumnName("ResolutionDeadline");

                    R.Property(x => x.Accomplished)
                    .HasColumnName("ResolutionAccomplished");

                });
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
