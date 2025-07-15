using Microsoft.AspNetCore.Identity;

namespace Fixeon.Auth.Infraestructure.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() { }

        public Guid CompanyId { get; set; }

        public Company Company { get; set; }
    }
}
