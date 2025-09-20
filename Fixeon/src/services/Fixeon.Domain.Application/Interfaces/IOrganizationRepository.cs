using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Entities;

namespace Fixeon.Domain.Application.Interfaces
{
    public interface IOrganizationRepository
    {
        public Task<List<Organization>> GetAllOrganizations();
        public Task<Organization> GetOrganizationById(Guid organizationId);
        public Task<List<Organization>> GetOrganizations(IEnumerable<Guid> organizationIds);

        public Task CreateOrganization(Organization organization);
        public Task UpdateOrganization(Organization organization);
        public Task DeleteOrganization(Organization organization);

        // SLA
        public Task<List<OrganizationsSLA>> GetSLAByOrganization(Guid organizationId);
        public Task AddOrganizationSLA(OrganizationsSLA organizationSLA);
        public Task UpdateSLA(OrganizationsSLA organizationSLA);
    }
}
