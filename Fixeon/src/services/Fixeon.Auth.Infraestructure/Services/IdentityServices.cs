using Fixeon.Auth.Application.Dtos.Requests;
using Fixeon.Auth.Application.Dtos.Responses;
using Fixeon.Auth.Application.Interfaces;
using Fixeon.Auth.Infraestructure.Entities;
using Fixeon.Auth.Infraestructure.Interfaces;

namespace Fixeon.Auth.Infraestructure.Services
{
    public class IdentityServices : IIdentityServices
    {
        private IIdentityRepository _authRepository;

        public IdentityServices(IIdentityRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<ApplicationUserResponse> AssociateRole(string email, string roleName)
        {
            var user = await _authRepository.FindByEmail(email);

            if (user is null)
                return new ApplicationUserResponse("Usuário não encotrado.");

            var role = await _authRepository.GetRole(roleName);

            if (role is null)
                return new ApplicationUserResponse("Perfil não encontrado.");

            var result = await _authRepository.AssociateRole(user, role.Name);

            if (result.Succeeded)
            {
                var roles = await _authRepository.GetRolesByUser(user);
                return new ApplicationUserResponse(user.Id, user.UserName, user.Email, roles);
            }

            return new ApplicationUserResponse(result.Errors.Select(e => e.Description).ToList());
        }

        public async Task<ApplicationUserResponse> CreateIdentityUser(CreateAccountRequest request)
        {
            var applicationUser = new ApplicationUser(request.Email, request.Username);

            var result = await _authRepository.CreateAccount(applicationUser, request.Password);

            if (result.Succeeded)
            {
                var user = await _authRepository.FindByEmail(request.Email);
                var roles = await _authRepository.GetRolesByUser(user);

                return new ApplicationUserResponse(user.Id, user.UserName, user.Email, roles);
            }

            return new ApplicationUserResponse(result.Errors.Select(e => e.Description).ToList());
        }

        public async Task<bool> CreateRole(string request)
        {
            var roleExists = await _authRepository.GetRole(request);

            if (roleExists != null)
                return false;

            var result = await _authRepository.CreateRole(request);

            return result.Succeeded;
        }

        public async Task<bool> FindUserByEmail(string email)
        {
            return await _authRepository.FindByEmail(email) is null;
        }

        public async Task<string> GenerateResetPasswordToken(string email)
        {
            var user = await _authRepository.FindByEmail(email);

            if (user is null)
                return null;

            var result = await _authRepository.GenerateResetPasswordToken(user);

            return result;
        }

        public async Task<List<ApplicationUserResponse>> GetAllUsers()
        {
            var users = await _authRepository.GetAllUsers();

            var tasks = users.Select(async u =>
            {
                var roles = await _authRepository.GetRolesByUser(u);
                return new ApplicationUserResponse(u.Id, u.UserName, u.Email, roles);
            });

            var result = await Task.WhenAll(tasks);

            return result.ToList();
        }

        public async Task<ApplicationUserResponse> GetuserById(string id)
        {
            var user = await _authRepository.FindById(id);

            if (user is null)
                return new ApplicationUserResponse("Usuário não encontrado.");

            var roles = await _authRepository.GetRolesByUser(user);

            return new ApplicationUserResponse(user.Id, user.UserName, user.Email, roles);
        }

        public async Task<ApplicationUserResponse> Login(string email, string password)
        {
            var user = await _authRepository.FindByEmail(email);
            var result = await _authRepository.Login(user, password);

            if (result.Succeeded)
            {
                var roles = await _authRepository.GetRolesByUser(user);
                return new ApplicationUserResponse(user.Id, user.UserName, user.Email, roles);
            }

            return new ApplicationUserResponse("Credenciais inválidas.");
        }

        public async Task<ApplicationUserResponse> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _authRepository.FindByEmail(request.Email);

            if (user is null)
                return new ApplicationUserResponse("Usuário não encontrado.");

            var result = await _authRepository.ResetPassword(user, request.Token, request.NewPassword);

            if (result.Succeeded)
                return new ApplicationUserResponse(user.Id, user.UserName, user.Email, null);

            return new ApplicationUserResponse(result.Errors.Select(e => e.Description).ToList());
        }
    }
}
