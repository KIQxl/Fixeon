using Fixeon.Auth.Infraestructure.Entities;
using Microsoft.AspNetCore.Identity;

namespace Fixeon.Auth.Infraestructure.Interfaces
{
    public interface IIdentityRepository
    {
        public Task<IdentityResult> CreateAccount(ApplicationUser request, string password, bool ignoreTenantInterceptor);
        public Task<IdentityResult> UpdateAccount(ApplicationUser applicationUser);
        public Task<SignInResult> Login(ApplicationUser user, string password);
        public Task<ApplicationUser> FindByEmail(string email);
        public Task<ApplicationUser> FindById(string id);
        public Task<IdentityResult> CreateRole(string roleName);
        public Task<IdentityResult> AssociateRole(ApplicationUser user, string role);
        public Task<List<ApplicationUser>> GetAllUsers(bool masterAdmin);
        public Task<ApplicationUser> GetUser(string email);
        public Task<string> GenerateResetPasswordToken(ApplicationUser user);
        public Task<IdentityResult> ResetPassword(ApplicationUser user, string token, string newPassword);
        public Task<List<ApplicationUser>> GetUsersByRoleName(string roleName);
        public Task<List<string>> GetRolesByUser(ApplicationUser user);
        public Task<IdentityRole> GetRole(string roleName);
        public Task<ApplicationUser> FindByEmailWithoutFilter(string email);
        public Task CreateOrganization(Organization organization);
        public Task<List<Organization>> GetAllOrganizations();
        public Task<Organization> GetOrganizationById(Guid organizationId);
        public Task DeleteOrganization(Organization organization);
    }
}
