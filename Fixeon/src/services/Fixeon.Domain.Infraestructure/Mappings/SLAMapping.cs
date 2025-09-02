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

            builder.Property(s => s.CompanyId)
                .IsRequired()
                .HasColumnType("varchar(36)");

            builder.Property(s => s.Organization)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(s => s.OrganizationId)
                .IsRequired()
                .HasColumnType("varchar(36)");

            builder.Property(s => s.SLAPriority)
                .IsRequired()
                .HasColumnType("varchar(30)");

            builder.Property(s => s.SLAInMinutes)
                .IsRequired()
                .HasColumnType("int");

            builder.Property(s => s.Type)
                .IsRequired()
                .HasColumnType("int");

            builder.Property(s => s.CreateAt)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(s => s.Type)
                .HasColumnType("datetime");
        }
    }
}
