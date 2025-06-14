using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fixeon.Domain.Infraestructure.Mappings
{
    public class AttachmentMapping : IEntityTypeConfiguration<Attachment>
    {
        public void Configure(EntityTypeBuilder<Attachment> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasColumnName("filename");

            builder.Property(a => a.Extension)
                .IsRequired()
                .HasColumnType("varchar(6)");

            builder.Property(a => a.UploadedAt)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(a => a.SenderId)
                    .IsRequired()
                    .HasColumnType("varchar(36)");

            builder.Property(a => a.TicketId);

            builder.Property(a => a.InteractionId);

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint(
                    "CK_Attachment_Ticket_Or_Interaction",
                    "(TicketId IS NOT NULL AND InteractionId IS NULL) OR (TicketId IS NULL AND InteractionId IS NOT NULL)"
                );
            });
        }
    }
}
