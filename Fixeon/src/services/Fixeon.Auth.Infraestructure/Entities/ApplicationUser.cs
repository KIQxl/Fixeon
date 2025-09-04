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
            CreatedAt = DateTime.Now;
        }

        public Guid CompanyId {  get; private set; }
        public Guid? OrganizationId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public void AssignCompany(Guid companyId) => CompanyId = companyId;

        public void AssignOrganization(Guid organizationId) => OrganizationId = organizationId;

        public void ChangeEmail(string email) => Email = email;
        public void ChangeUserName(string userName) => UserName = userName;
    }
}
