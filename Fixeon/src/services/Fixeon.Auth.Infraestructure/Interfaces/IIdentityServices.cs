using Fixeon.Auth.Application.Dtos.Responses;
using Fixeon.Auth.Infraestructure.Dtos.Requests;
using Fixeon.Auth.Infraestructure.Dtos.Responses;

namespace Fixeon.Auth.Infraestructure.Interfaces
{
    public interface IIdentityServices
    {
        public Task<Response<ApplicationUserResponse>> AssociateRole(string email, string roleName);
        public Task<Response<ApplicationUserResponse>> CreateIdentityUser(CreateAccountRequest request);
        public Task<Response<bool>> CreateRole(string request);
        public Task<bool> FindUserByEmail(string email);
        public Task<Response<bool>> GenerateResetPasswordToken(string email);
        public Task<Response<List<ApplicationUserResponse>>> GetAllUsers();
        public Task<Response<ApplicationUserResponse>> GetuserById(string id);
        public Task<Response<LoginResponse>> Login(string email, string password);
        public Task<Response<ApplicationUserResponse>> ResetPassword(ResetPasswordRequest request);
    }
}
