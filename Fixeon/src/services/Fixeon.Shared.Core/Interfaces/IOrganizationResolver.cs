using Fixeon.Shared.Core.Models;

namespace Fixeon.Shared.Core.Interfaces
{
    public interface IOrganizationResolver
    {
        public Task<CurrentOrganization> GetOrganization(Guid organizationId);
    }
}
