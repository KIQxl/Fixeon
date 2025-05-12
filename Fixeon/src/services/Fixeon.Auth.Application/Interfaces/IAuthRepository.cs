using Fixeon.Auth.Application.Dtos;

namespace Fixeon.Auth.Application.Interfaces
{
    public interface IAuthRepository
    {
        Task<ApplicationUser> CreateAccount(CreateAccountRequest request);
        Task<ApplicationUser> Login(string email, string password);
        Task<bool> FindByEmail(string email);
    }
}
