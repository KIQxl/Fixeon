﻿using Fixeon.Auth.Application.Dtos.Requests;
using Fixeon.Auth.Application.Dtos.Responses;

namespace Fixeon.Auth.Application.Interfaces
{
    public interface IAuthenticationServices
    {
        public Task<Response<LoginResponse>> Login(LoginRequest request);
        public Task<Response<LoginResponse>> CreateAccount(CreateAccountRequest request);
        public Task<Response<bool>> CreateRole(string request);
        public Task<Response<LoginResponse>> AssociateRole(string userId, string role);
        public Task<Response<ApplicationUser>> GetUserEmailAsync(string userId);
        public Task<Response<List<ApplicationUser>>> GetAllUsersAsync();
        public Task<Response<bool>> SendRecoveryPasswordLink(string email);
        public Task<Response<ApplicationUser>> ResetPassword(ResetPasswordRequest request);
    }
}
