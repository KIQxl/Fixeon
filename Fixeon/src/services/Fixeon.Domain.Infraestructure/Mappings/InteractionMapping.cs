using Fixeon.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fixeon.Domain.Infraestructure.Mappings
{
    public class InteractionMapping : IEntityTypeConfiguration<Interaction>
    {
        public void Configure(EntityTypeBuilder<Interaction> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Message)
                .IsRequired()
                .HasColumnType("varchar(500)");

            builder.Property(i => i.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(i => i.TicketId)
                .IsRequired();

            builder.OwnsOne(i => i.CreatedBy, interactionUser =>
            {
                interactionUser.Property(u => u.UserId)
                .IsRequired()
                .HasColumnType("varchar(36)")
                .HasColumnName("userId");

                interactionUser.Property(u => u.UserName)
                .IsRequired()
                .HasColumnType("varchar(100)")
                .HasColumnName("username");
            });

            builder.HasMany(i => i.Attachments)
                .WithOne(a => a.Interaction)
                .HasForeignKey(a => a.InteractionId);
        }
    }
}
