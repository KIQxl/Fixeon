using Fixeon.Auth.Infraestructure.Dtos.Responses;
using Fixeon.Auth.Infraestructure.Interfaces;
using Fixeon.Domain.Application.Interfaces;

namespace Fixeon.ACL.DomainContextQueries
{
    public class OrganizationQueries : IOrganizationACLQueries
    {
        private readonly IOrganizationServices _orgServices;

        public OrganizationQueries(IOrganizationServices orgServices)
        {
            _orgServices = orgServices;
        }

        public async Task<UserOrganizationResponse> GetOrganizationByIdAsync(Guid organizationId)
        {
            var org = await _orgServices.GetOrganizationById(organizationId);

            if (org is null)
                return null;

            var userOrganization = new UserOrganizationResponse { OrganizationId = org.Data.Id, OrganizationName = org.Data.Name };

            return userOrganization;
        }
    }
}
