using Microsoft.AspNetCore.Identity;

namespace Fixeon.Auth.Infraestructure.Entities
{
    public class ApplicationUser : IdentityUser, ITenantEntity
    {
        protected ApplicationUser() { }

        public ApplicationUser(string email, string username, string phoneNumber, string jobTitle, string? profilePictureUrl)
        {
            Email = email;
            UserName = username;
            CreatedAt = DateTime.Now;
            Active = true;
            PhoneNumber = phoneNumber;
            JobTitle = jobTitle;
            ProfilePictureUrl = profilePictureUrl;
        }

        public Guid? CompanyId {  get; private set; }
        public Guid? OrganizationId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool Active { get; private set; }
        public string? JobTitle { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public void AssignCompany(Guid companyId) => CompanyId = companyId;

        public void AssignOrganization(Guid organizationId) => OrganizationId = organizationId;

        public void ChangeEmail(string email) => Email = email;
        public void ChangeUserName(string userName) => UserName = userName;
    }
}
