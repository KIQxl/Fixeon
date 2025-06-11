using Fixeon.Auth.Application.Dtos;

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
    }
}
