using Fixeon.Auth.Application.Dtos.Requests;
using Fixeon.Auth.Application.Dtos.Responses;

namespace Fixeon.Auth.Application.Interfaces
{
    public interface IAuthRepository
    {
        Task<ApplicationUserResponse> CreateAccount(CreateAccountRequest request);
        Task<ApplicationUserResponse> Login(string email, string password);
        Task<bool> FindByEmail(string email);
        Task<bool> CreateRole(string role);
        Task<ApplicationUserResponse> AssociateRole(string userId, string role);
        Task<List<ApplicationUserResponse>> GetAllUsers();
        Task<ApplicationUserResponse> GetUser(string email);
        public Task<string> GenerateResetPasswordToken(string email);
        public Task<ApplicationUserResponse> ResetPassword(ResetPasswordRequest request);
        public Task<List<ApplicationUserResponse>> GetUsersByRoleName(string roleName);
    }
}
