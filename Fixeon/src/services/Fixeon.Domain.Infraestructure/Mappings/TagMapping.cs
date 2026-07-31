using Fixeon.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fixeon.Domain.Infraestructure.Mappings
{
    public class TagMapping : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasOne(t => t.Company)
                .WithMany(c => c.Tags)
                .HasConstraintName("CompanyTags");

            builder.HasMany(t => t.Tickets)
              .WithMany(t => t.Tags)
              .UsingEntity<Dictionary<string, object>>(
                  "tickettags",
                  j => j.HasOne<Ticket>()
                        .WithMany()
                        .HasForeignKey("ticketid")
                        .HasPrincipalKey(nameof(Ticket.Id)),
                  j => j.HasOne<Tag>()
                        .WithMany()
                        .HasForeignKey("tagid")
                        .HasPrincipalKey(nameof(Tag.Id)),
                  j => {
                      j.Property<Guid>("ticketid").HasColumnName("ticketid");
                      j.Property<Guid>("tagid").HasColumnName("tagid");
                      j.ToTable("tickettags");
                  }
              );
        }
    }
}
