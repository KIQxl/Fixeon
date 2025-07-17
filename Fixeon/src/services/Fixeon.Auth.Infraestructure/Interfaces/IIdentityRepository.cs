using Fixeon.Auth.Infraestructure.Entities;
using Microsoft.AspNetCore.Identity;

namespace Fixeon.Auth.Infraestructure.Interfaces
{
    public interface IIdentityRepository
    {
        Task<IdentityResult> CreateAccount(ApplicationUser request, string password);
        Task<SignInResult> Login(ApplicationUser user, string password);
        Task<ApplicationUser> FindByEmail(string email);
        Task<ApplicationUser> FindById(string id);
        Task<IdentityResult> CreateRole(string roleName);
        Task<IdentityResult> AssociateRole(ApplicationUser user, string role);
        Task<List<ApplicationUser>> GetAllUsers();
        Task<ApplicationUser> GetUser(string email);
        public Task<string> GenerateResetPasswordToken(ApplicationUser user);
        public Task<IdentityResult> ResetPassword(ApplicationUser user, string token, string newPassword);
        public Task<List<ApplicationUser>> GetUsersByRoleName(string roleName);
        public Task<List<string>> GetRolesByUser(ApplicationUser user);
        public Task<IdentityRole> GetRole(string roleName);
    }
}
