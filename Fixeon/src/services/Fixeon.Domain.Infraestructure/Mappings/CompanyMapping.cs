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
                .HasMaxLength(200);

            builder.Property(x => x.CNPJ)
                .IsRequired()
                .HasMaxLength(14);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasMaxLength(13);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(25)
                .HasConversion<string>();

            builder.OwnsOne(x => x.Address, address =>
            {
                address.Property(x => x.Street)
                .HasMaxLength(100)
                .HasColumnName("Street")
                .IsRequired();

                address.Property(x => x.Number)
                .HasMaxLength(6)
                .HasColumnName("Number")
                .IsRequired();

                address.Property(x => x.Neighborhood)
                .HasMaxLength(100)
                .HasColumnName("Neighborhood")
                .IsRequired();

                address.Property(x => x.City)
                .HasMaxLength(50)
                .HasColumnName("City")
                .IsRequired();

                address.Property(x => x.State)
                .HasMaxLength(50)
                .HasColumnName("State")
                .IsRequired();

                address.Property(x => x.PostalCode)
                .HasMaxLength(8)
                .HasColumnName("PostalCode")
                .IsRequired();

                address.Property(x => x.Country)
                .HasMaxLength(50)
                .HasColumnName("Country")
                .IsRequired();
            });

            builder.Property(x => x.ProfilePictureUrl)
                .HasMaxLength(1000);

            builder.HasIndex(x => x.CNPJ).IsUnique();
        }
    }
}
