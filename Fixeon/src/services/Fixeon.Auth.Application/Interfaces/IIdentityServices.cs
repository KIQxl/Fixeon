using Fixeon.Auth.Application.Dtos.Requests;
using Fixeon.Auth.Application.Dtos.Responses;

namespace Fixeon.Auth.Application.Interfaces
{
    public interface IIdentityServices
    {
        public Task<ApplicationUserResponse> CreateIdentityUser(CreateAccountRequest request);
        public Task<bool> FindUserByEmail(string email);
        public Task<ApplicationUserResponse> Login(string email, string password);
        public Task<bool> CreateRole(string request);
        public Task<ApplicationUserResponse> AssociateRole(string email, string roleName);
        public Task<ApplicationUserResponse> GetuserById(string id);
        public Task<List<ApplicationUserResponse>> GetAllUsers();
        public Task<string> GenerateResetPasswordToken(string email);
        public Task<ApplicationUserResponse> ResetPassword(ResetPasswordRequest request);
    }
}
