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
        public Task<IdentityResult> AssociateRoles(ApplicationUser user, List<string> roles);
        public Task<IdentityResult> RemoveRoles(ApplicationUser user, List<string> roles);
        public Task<List<ApplicationUser>> GetAllUsers(bool masterAdmin);
        public Task<List<IdentityRole>> GetAllRoles();
        public Task<List<IdentityRole>> GetRolesByName(List<string> rolesName);
        public Task<ApplicationUser> GetUser(string email);
        public Task<string> GenerateResetPasswordToken(ApplicationUser user);
        public Task<IdentityResult> ResetPassword(ApplicationUser user, string token, string newPassword);
        public Task<List<ApplicationUser>> GetUsersByRoleName(string roleName);
        public Task<List<string>> GetRolesByUser(ApplicationUser user);
        public Task<IdentityRole> GetRole(string roleName);
        public Task<ApplicationUser> FindByEmailWithoutFilter(string email);
    }
}
