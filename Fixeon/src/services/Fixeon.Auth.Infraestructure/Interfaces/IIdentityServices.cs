using Fixeon.Auth.Application.Dtos.Responses;
using Fixeon.Auth.Infraestructure.Dtos.Requests;
using Fixeon.Auth.Infraestructure.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fixeon.Auth.Infraestructure.Interfaces
{
    public interface IIdentityServices
    {
        public Task<Response<ApplicationUserResponse>> AssociateRole(string email, List<string> roleName);
        public Task<Response<ApplicationUserResponse>> CreateIdentityUser(CreateAccountRequest request);
        public Task<Response<ApplicationUserResponse>> UpdateApplicationUser(UpdateApplicationUserRequest request);
        public Task<Response<bool>> CreateRole(string request);
        public Task<bool> FindUserByEmail(string email);
        public Task<Response<bool>> GenerateResetPasswordToken(string email);
        public Task<Response<List<ApplicationUserResponse>>> GetAllUsers(Guid? id, string? email, Guid? organization, string? username);
        public Task<Response<List<string>>> GetAllRoles();
        public Task<Response<ApplicationUserResponse>> GetuserById(string id);
        public Task<Response<LoginResponse>> Login(string email, string password);
        public Task<Response<ApplicationUserResponse>> ResetPassword(ResetPasswordRequest request);
        public Task<Response<bool>> CreateMasterAdmin(CreateAccountRequest request);
        public Task<Response<ApplicationUserResponse>> MasterAdminCreateFirstForCompany(CreateAccountRequest request);
        public Task<Response<List<ApplicationUserResponse>>> MasterAdminGetAllUsers();
        public Task<Response<List<ApplicationUserResponse>>> GetUserByRoleName(string role);
    }
}
