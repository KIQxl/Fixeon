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

            builder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasColumnType("varchar(13)");

            builder.Property(x => x.Status)
                .IsRequired()
                .HasColumnType("varchar(25)")
                .HasConversion<string>();

            builder.OwnsOne(x => x.Address, address =>
            {
                address.Property(x => x.Street)
                .HasColumnType("varchar(100)")
                .HasColumnName("Street")
                .IsRequired();

                address.Property(x => x.Number)
                .HasColumnType("varchar(6)")
                .HasColumnName("Number")
                .IsRequired();

                address.Property(x => x.Neighborhood)
                .HasColumnType("varchar(100)")
                .HasColumnName("Neighborhood")
                .IsRequired();

                address.Property(x => x.City)
                .HasColumnType("varchar(50)")
                .HasColumnName("City")
                .IsRequired();

                address.Property(x => x.State)
                .HasColumnType("varchar(50)")
                .HasColumnName("State")
                .IsRequired();

                address.Property(x => x.PostalCode)
                .HasColumnType("varchar(8)")
                .HasColumnName("PostalCode")
                .IsRequired();

                address.Property(x => x.Country)
                .HasColumnType("varchar(50)")
                .HasColumnName("Country")
                .IsRequired();
            });

            builder.Property(x => x.ProfilePictureUrl)
                .HasColumnType("varchar(1000)");

            builder.HasIndex(x => x.CNPJ).IsUnique();
        }
    }
}
