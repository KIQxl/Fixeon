using Fixeon.Shared.Core.Interfaces;
using Fixeon.Shared.Core.Models;

namespace Fixon.Tests.MockRepository
{
    internal class FakeOrganizationResolver : IOrganizationResolver
    {
        public Task<OrganizationResolverView> GetOrganization(Guid organizationId)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrganizationResolverView>> GetOrganizations(IEnumerable<Guid> organizationIds)
        {
            throw new NotImplementedException();
        }

        public Task<List<SLAResolverView>> GetSLAByOrganization(Guid organizationId)
        {
            throw new NotImplementedException();
        }
    }

    internal class FakeCompanyResolver : ICompanyResolver
    {
        public Task<CompanyResolverView> GetCompany(Guid companyId)
        {
            throw new NotImplementedException();
        }
    }
}
