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

        public Company Company { get; set; }

        public Guid CompanyId {  get; private set; }
    }
}
