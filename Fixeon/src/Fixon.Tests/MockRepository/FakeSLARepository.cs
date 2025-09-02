using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Core.Enums;

namespace Fixon.Tests.MockRepository
{
    public class FakeSLARepository : ISLARepository
    {
        public Task AddOrganizationSLA(OrganizationsSLA organizationSLA)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrganizationsSLA>> GetSLAByOrganizationAndPriority(Guid organizationId, EPriority priority)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrganizationSLA(OrganizationsSLA organizationSLA)
        {
            throw new NotImplementedException();
        }
    }
}
