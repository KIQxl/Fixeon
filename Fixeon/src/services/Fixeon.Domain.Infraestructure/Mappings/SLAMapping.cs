using Fixeon.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fixeon.Domain.Infraestructure.Mappings
{
    public class SLAMapping : IEntityTypeConfiguration<OrganizationsSLA>
    {
        public void Configure(EntityTypeBuilder<OrganizationsSLA> builder)
        {
            throw new NotImplementedException();
        }
    }
}
