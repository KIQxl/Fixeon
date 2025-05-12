using Fixeon.Auth.Application.Dtos;

namespace Fixeon.Auth.Application.Interfaces
{
    public interface IAuthenticationServices
    {
        public Task<Response<LoginResponse>> Login(LoginRequest request);
        public Task<Response<LoginResponse>> CreateAccount(CreateAccountRequest request);
    }
}
