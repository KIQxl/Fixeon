using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Core.Enums;

namespace Fixeon.Domain.Application.Interfaces
{
    public interface ISLARepository
    {
        public Task<List<OrganizationsSLA>> GetSLAByOrganizationAndPriority(Guid organizationId, EPriority priority);

        public Task AddOrganizationSLA(OrganizationsSLA organizationSLA);

        public void UpdateOrganizationSLA(OrganizationsSLA organizationSLA);
    }
}
