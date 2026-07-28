using Fixeon.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fixeon.Domain.Infraestructure.Mappings
{
    public class SLAMapping : IEntityTypeConfiguration<OrganizationsSLA>
    {
        public void Configure(EntityTypeBuilder<OrganizationsSLA> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.OrganizationId)
                .IsRequired();

            builder.Property(s => s.SLAPriority)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(s => s.SLAInMinutes)
                .IsRequired();

            builder.Property(s => s.Type)
                .IsRequired();

            builder.Property(s => s.CreateAt)
                .IsRequired();

            builder.Property(s => s.ModifiedAt);
        }
    }
}
