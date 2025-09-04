using Fixeon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fixeon.Domain.Infraestructure.Mappings
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

            builder.Property(x => x.Email)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.HasIndex(x => x.CNPJ).IsUnique();
        }
    }
}
