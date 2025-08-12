using Microsoft.AspNetCore.Identity;

namespace Fixeon.Auth.Infraestructure.Entities
{
    public class ApplicationUser : IdentityUser, ITenantEntity
    {
        protected ApplicationUser() { }

        public ApplicationUser(string email, string username)
        {
            Email = email;
            UserName = username;
        }

        public Company Company { get; private set; }

        public Guid CompanyId {  get; private set; }
        public Organization? Organization { get; private set; }
        public Guid? OrganizationId { get; private set; }

        public void AssignCompany(Guid companyId) => CompanyId = companyId;

        public void AssignOrgazation(Guid organizationId) => OrganizationId = organizationId;
    }
}
