using Fixeon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fixeon.Domain.Infraestructure.Mappings
{
    public class OrganizationMapping : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(x => x.CNPJ)
                .IsRequired()
                .HasColumnType("varchar(14)");

            builder.Property(x => x.Email)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasColumnType("date");

            builder.HasOne(o => o.Company)
                .WithMany(c => c.Organizations)
                .HasForeignKey(o => o.CompanyId);

            builder.HasMany(o => o.SLAs)
                .WithOne(o => o.Organization)
                .HasForeignKey(o => o.OrganizationId);
        }
    }
}
