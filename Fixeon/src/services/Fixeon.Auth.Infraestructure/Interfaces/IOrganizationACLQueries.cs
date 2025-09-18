using Fixeon.Auth.Infraestructure.Dtos.Responses;

namespace Fixeon.Auth.Infraestructure.Interfaces
{
    public interface IOrganizationACLQueries
    {
        public Task<UserOrganizationResponse> GetOrganizationByIdAsync(Guid organizationId);
    }
}
