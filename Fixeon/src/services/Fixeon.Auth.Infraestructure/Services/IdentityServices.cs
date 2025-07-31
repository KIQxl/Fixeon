using Fixeon.Auth.Application.Dtos.Responses;
using Fixeon.Auth.Infraestructure.Dtos.Requests;
using Fixeon.Auth.Infraestructure.Dtos.Responses;
using Fixeon.Auth.Infraestructure.Entities;
using Fixeon.Auth.Infraestructure.Interfaces;
using Fixeon.Shared.Core.Interfaces;
using Fixeon.Shared.Core.Models;

namespace Fixeon.Auth.Infraestructure.Services
{
    public class IdentityServices : IIdentityServices
    {
        private readonly IIdentityRepository _authRepository;
        private readonly IBackgroundEmailJobWrapper _backgroundEmailJobWrapper;
        private readonly IUrlEncoder _urlEncoder;
        private readonly ITokenGeneratorService _tokenGeneratorService;
        private readonly ICompanyRepository _companyRepository;

        public IdentityServices(IIdentityRepository authRepository, IBackgroundEmailJobWrapper backgroundEmailJobWrapper, IUrlEncoder urlEncoder, ITokenGeneratorService tokenGeneratorService, ICompanyRepository companyRepository)
        {
            _authRepository = authRepository;
            _backgroundEmailJobWrapper = backgroundEmailJobWrapper;
            _urlEncoder = urlEncoder;
            _tokenGeneratorService = tokenGeneratorService;
            _companyRepository = companyRepository;
        }

        // GET
        public async Task<Response<List<ApplicationUserResponse>>> GetAllUsers()
        {
            var users = await _authRepository.GetAllUsers(false);

            if (users is null)
                return new Response<List<ApplicationUserResponse>>("Nenhum usuário encontrado.");

            var response = new List<ApplicationUserResponse>();

            foreach (var u in users)
            {
                var roles = await _authRepository.GetRolesByUser(u);
                response.Add(new ApplicationUserResponse(u.Id, u.UserName, u.Email, roles));
            }

            return new Response<List<ApplicationUserResponse>>(response);
        }

        public async Task<Response<ApplicationUserResponse>> GetuserById(string id)
        {
            var user = await _authRepository.FindById(id);

            if (user is null)
                return new Response<ApplicationUserResponse>("Usuário não encontrado.");

            var roles = await _authRepository.GetRolesByUser(user);

            return new Response<ApplicationUserResponse>(new ApplicationUserResponse(user.Id, user.UserName, user.Email, roles));
        }

        public async Task<bool> FindUserByEmail(string email)
        {
            return await _authRepository.FindByEmail(email) is null;
        }

        // CREATE
        public async Task<Response<ApplicationUserResponse>> CreateIdentityUser(CreateAccountRequest request)
        {
            try
            {
                var applicationUser = new ApplicationUser(request.Email, request.Username);

                var result = await _authRepository.CreateAccount(applicationUser, request.Password, false);

                if (result.Succeeded)
                {
                    var user = await _authRepository.FindByEmail(request.Email);
                    var roles = await _authRepository.GetRolesByUser(user);

                    _backgroundEmailJobWrapper.SendEmail(new EmailMessage { To = user.Email, Subject = "Bem-vindo! - Fixeon", Body = EmailDictionary.WelcomeEmail });

                    return new Response<ApplicationUserResponse>(new ApplicationUserResponse(user.Id, user.UserName, user.Email, roles));
                }

                return new Response<ApplicationUserResponse>(result.Errors.Select(e => e.Description).ToList());
            }
            catch (Exception ex)
            {
                return new Response<ApplicationUserResponse>(new List<string> { "Ocorreu um erro.", ex.InnerException?.Message ?? ex.Message });
            }
        }

        public async Task<Response<bool>> CreateRole(string request)
        {
            var roleExists = await _authRepository.GetRole(request);

            if (roleExists != null)
                return new Response<bool>("Perfil já está cadastrado.");

            var result = await _authRepository.CreateRole(request);

            if (result.Succeeded)
                return new Response<bool>(true);

            return new Response<bool>(result.Errors.Select(e => e.Description).ToList());
        }


        // AUTH CONTEXT ACTIONS
        public async Task<Response<LoginResponse>> Login(string email, string password)
        {
            var user = await _authRepository.FindByEmailWithoutFilter(email);

            if (user is null)
                return new Response<LoginResponse>("Usuário não encotrado.");

            var result = await _authRepository.Login(user, password);

            if (result.Succeeded)
            {
                var roles = await _authRepository.GetRolesByUser(user);

                var token = _tokenGeneratorService.GenerateToken(user, roles);

                return new Response<LoginResponse>(new LoginResponse(user.Id, user.UserName, user.Email, token, roles));
            }

            return new Response<LoginResponse>("Credenciais inválidas.");
        }

        public async Task<Response<ApplicationUserResponse>> AssociateRole(string userId, string roleName)
        {
            var user = await _authRepository.FindById(userId);

            if (user is null)
                return new Response<ApplicationUserResponse>("Usuário não encontrado.");

            var role = await _authRepository.GetRole(roleName);

            if (role is null)
                return new Response<ApplicationUserResponse>("Perfil não encontrado.");

            var result = await _authRepository.AssociateRole(user, role.Name);

            if (result.Succeeded)
            {
                var roles = await _authRepository.GetRolesByUser(user);
                return new Response<ApplicationUserResponse>(new ApplicationUserResponse(user.Id, user.UserName, user.Email, roles));
            }

            return new Response<ApplicationUserResponse>(result.Errors.Select(e => e.Description).ToList());
        }

        public async Task<Response<bool>> GenerateResetPasswordToken(string email)
        {
            var applicationUser = await _authRepository.FindByEmail(email);

            if (applicationUser is null)
                return new Response<bool>("Usuário não encontrado.");

            var token = await _authRepository.GenerateResetPasswordToken(applicationUser);

            if (token is null)
                return new Response<bool>("Não foi possivel gerar o código de recuperação.");

            var encodedToken = $"?recovery-token={_urlEncoder.Encode(token)}";

            _backgroundEmailJobWrapper.SendEmail(new EmailMessage { To = email, Subject = "Recuperação de Senha - Fixeon", Body = EmailDictionary.ResetPasswordEmail.Replace("{{reset_link}}", encodedToken) });

            return new Response<bool>(true);
        }

        public async Task<Response<ApplicationUserResponse>> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _authRepository.FindByEmail(request.Email);

            if (user is null)
                return new Response<ApplicationUserResponse>("Usuário não encontrado.");

            var result = await _authRepository.ResetPassword(user, request.Token, request.NewPassword);

            if (result.Succeeded)
                return new Response<ApplicationUserResponse>(new ApplicationUserResponse(user.Id, user.UserName, user.Email, null));

            return new Response<ApplicationUserResponse>(result.Errors.Select(e => e.Description).ToList());
        }

        //MASTER ADMIN ACTIONS
        public async Task<Response<ApplicationUserResponse>> MasterAdminCreateFirstForCompany(CreateAccountRequest request)
        {
            try
            {
                var company = await _companyRepository.GetCompanyById(request.CompanyId.Value);

                var applicationUser = new ApplicationUser(request.Email, request.Username);
                applicationUser.AssignCompany(request.CompanyId.Value);

                var result = await _authRepository.CreateAccount(applicationUser, request.Password, true);

                if (result.Succeeded)
                {
                    var user = await _authRepository.FindByEmailWithoutFilter(request.Email);
                    var roles = await _authRepository.GetRolesByUser(user);

                    _backgroundEmailJobWrapper.SendEmail(new EmailMessage { To = user.Email, Subject = "Bem-vindo! - Fixeon", Body = EmailDictionary.WelcomeEmail });

                    return new Response<ApplicationUserResponse>(new ApplicationUserResponse(user.Id, user.UserName, user.Email, roles));
                }

                return new Response<ApplicationUserResponse>(result.Errors.Select(e => e.Description).ToList());
            }
            catch (Exception ex)
            {
                return new Response<ApplicationUserResponse>(new List<string> { "Ocorreu um erro.", ex.InnerException?.Message ?? ex.Message });
            }
        }

        public async Task<Response<List<ApplicationUserResponse>>> MasterAdminGetAllUsers()
        {
            var users = await _authRepository.GetAllUsers(true);

            if (users is null)
                return new Response<List<ApplicationUserResponse>>("Nenhum usuário encontrado.");

            var response = new List<ApplicationUserResponse>();

            foreach (var u in users)
            {
                var roles = await _authRepository.GetRolesByUser(u);
                response.Add(new ApplicationUserResponse(u.Id, u.UserName, u.Email, roles));
            }

            return new Response<List<ApplicationUserResponse>>(response);
        }
    }
}
