using Fixeon.Auth.Infraestructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fixeon.Auth.Infraestructure.Mappings
{
    public class CompanyMapping : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(x => x.CNPJ)
                .IsRequired()
                .HasColumnType("varchar(14)");

            builder.HasMany(x => x.Users)
                .WithOne(x => x.Company)
                .HasForeignKey(x => x.CompanyId);

            builder.HasIndex(x => x.CNPJ).IsUnique();
        }
    }
}
