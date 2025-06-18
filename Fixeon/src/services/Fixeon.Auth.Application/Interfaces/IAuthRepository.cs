using Fixeon.Auth.Application.Dtos.Requests;
using Fixeon.Auth.Application.Dtos.Responses;

namespace Fixeon.Auth.Application.Interfaces
{
    public interface IAuthRepository
    {
        Task<ApplicationUser> CreateAccount(CreateAccountRequest request);
        Task<ApplicationUser> Login(string email, string password);
        Task<bool> FindByEmail(string email);
        Task<bool> CreateRole(string role);
        Task<ApplicationUser> AssociateRole(string userId, string role);
        Task<List<ApplicationUser>> GetAllUsers();
        Task<ApplicationUser> GetUser(string email);
        public Task<string> GenerateResetPasswordToken(string email);
        public Task<ApplicationUser> ResetPassword(ResetPasswordRequest request);
    }
}
