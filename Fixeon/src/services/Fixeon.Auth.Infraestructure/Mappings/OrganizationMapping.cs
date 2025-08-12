using Fixeon.Auth.Infraestructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fixeon.Auth.Infraestructure.Mappings
{
    public class OrganizationMapping : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.HasOne(o => o.Company)
                .WithMany(c => c.Organizations)
                .HasForeignKey(o => o.CompanyId);

            builder.HasMany(o => o.Users)
                .WithOne(u => u.Organization)
                .HasForeignKey(o => o.OrganizationId);
        }
    }
}
