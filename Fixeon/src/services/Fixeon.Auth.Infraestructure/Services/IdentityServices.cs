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
        private readonly IOrganizationACLQueries _OrganizationQueries;

        public IdentityServices(IIdentityRepository authRepository, IBackgroundEmailJobWrapper backgroundEmailJobWrapper, IUrlEncoder urlEncoder, ITokenGeneratorService tokenGeneratorService, IOrganizationACLQueries organizationQueries)
        {
            _authRepository = authRepository;
            _backgroundEmailJobWrapper = backgroundEmailJobWrapper;
            _urlEncoder = urlEncoder;
            _tokenGeneratorService = tokenGeneratorService;
            _OrganizationQueries = organizationQueries;
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
                UserOrganizationResponse organizationResponse = new UserOrganizationResponse();

                if (u.OrganizationId.HasValue)
                {
                    var org = await _OrganizationQueries.GetOrganizationByIdAsync(u.OrganizationId.Value);

                    if(org != null)
                    {
                        organizationResponse.OrganizationId = org.OrganizationId;
                        organizationResponse.OrganizationName = org.OrganizationName;
                    }
                }


                response.Add(new ApplicationUserResponse(u.Id, u.UserName, u.Email, organizationResponse, roles));
            }

            return new Response<List<ApplicationUserResponse>>(response);
        }

        public async Task<Response<ApplicationUserResponse>> GetuserById(string id)
        {
            var user = await _authRepository.FindById(id);

            if (user is null)
                return new Response<ApplicationUserResponse>("Usuário não encontrado.");

            var roles = await _authRepository.GetRolesByUser(user);

            UserOrganizationResponse organizationResponse = new UserOrganizationResponse();

            if (user.OrganizationId.HasValue)
            {
                var org = await _OrganizationQueries.GetOrganizationByIdAsync(user.OrganizationId.Value);

                if (org != null)
                {
                    organizationResponse.OrganizationId = org.OrganizationId;
                    organizationResponse.OrganizationName = org.OrganizationName;
                }
            }

            return new Response<ApplicationUserResponse>(new ApplicationUserResponse(user.Id, user.UserName, user.Email, organizationResponse, roles));
        }

        public async Task<Response<List<ApplicationUserResponse>>> GetUserByRoleName(string role)
        {
            var users = await _authRepository.GetUsersByRoleName(role);

            if (users is null)
                return new Response<List<ApplicationUserResponse>>("Nenhum usuário encontrado.");

            var response = new List<ApplicationUserResponse>();

            foreach (var u in users)
            {
                var roles = await _authRepository.GetRolesByUser(u);
                UserOrganizationResponse organizationResponse = new UserOrganizationResponse();

                if (u.OrganizationId.HasValue)
                {
                    var org = await _OrganizationQueries.GetOrganizationByIdAsync(u.OrganizationId.Value);

                    if (org != null)
                    {
                        organizationResponse.OrganizationId = org.OrganizationId;
                        organizationResponse.OrganizationName = org.OrganizationName;
                    }
                }


                response.Add(new ApplicationUserResponse(u.Id, u.UserName, u.Email, organizationResponse, roles));
            }

            return new Response<List<ApplicationUserResponse>>(response);
        }

        public async Task<bool> FindUserByEmail(string email)
        {
            return await _authRepository.FindByEmail(email) is null;
        }

        public async Task<Response<List<string>>> GetAllRoles()
        {
            var roles = await _authRepository.GetAllRoles();

            if (roles is null)
                return new Response<List<string>>("Nenhum perfil encontrado.");

            roles.Remove(roles.FirstOrDefault(r => r.Name == "MasterAdmin"));

            return new Response<List<string>>(roles.Select(r => r.Name).ToList(), true);
        }

        // CREATE
        public async Task<Response<ApplicationUserResponse>> CreateIdentityUser(CreateAccountRequest request)
        {
            try
            {
                var user = new ApplicationUser(request.Email, request.Username);

                if (request.OrganizationId.HasValue)
                    user.AssignOrganization(request.OrganizationId.Value);

                var result = await _authRepository.CreateAccount(user, request.Password, false);

                if (result.Succeeded)
                {
                    var roles = await _authRepository.GetRolesByName(request.Roles);

                    if (roles.Any())
                        await _authRepository.AssociateRoles(user, roles.Select(r => r.Name).ToList());

                    UserOrganizationResponse organizationResponse = new UserOrganizationResponse();

                    if (user.OrganizationId.HasValue)
                    {
                        var org = await _OrganizationQueries.GetOrganizationByIdAsync(user.OrganizationId.Value);

                        if (org != null)
                        {
                            organizationResponse.OrganizationId = org.OrganizationId;
                            organizationResponse.OrganizationName = org.OrganizationName;
                        }
                    }

                    _backgroundEmailJobWrapper.SendEmail(new EmailMessage { To = user.Email, Subject = "Bem-vindo! - Fixeon", Body = EmailDictionary.WelcomeEmail });

                    return new Response<ApplicationUserResponse>(new ApplicationUserResponse(user.Id, user.UserName, user.Email, organizationResponse, roles.Select(r => r.Name).ToList()));
                }

                return new Response<ApplicationUserResponse>(result.Errors.Select(e => e.Description).ToList());
            }
            catch (Exception ex)
            {
                return new Response<ApplicationUserResponse>(new List<string> { "Ocorreu um erro.", ex.InnerException?.Message ?? ex.Message });
            }
        }

        public async Task<Response<ApplicationUserResponse>> UpdateApplicationUser(UpdateApplicationUserRequest request)
        {
            try
            {
                var user = await _authRepository.FindById(request.Id.ToString());

                if (user != null)
                {
                    if (!string.IsNullOrEmpty(request.UserName))
                        user.ChangeUserName(request.UserName);

                    if (!string.IsNullOrEmpty(request.Email))
                        user.ChangeEmail(request.Email);

                    if (request.OrganizationId.HasValue)
                        user.AssignOrganization(request.OrganizationId.Value);

                    var result = await _authRepository.UpdateAccount(user);

                    if (result.Succeeded)
                        return new Response<ApplicationUserResponse>(new ApplicationUserResponse(user.Id, user.UserName, user.Email, null, null));

                    return new Response<ApplicationUserResponse>(result.Errors
                        .Select(e => e.Description)
                        .ToList());
                }

                return new Response<ApplicationUserResponse>("Usuário não encontrado.");

            }
            catch (Exception ex)
            {
                return new Response<ApplicationUserResponse>(ex.Message);
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
                return new Response<LoginResponse>("Usuário não encontrado.");

            var result = await _authRepository.Login(user, password);

            if (result.Succeeded)
            {
                var roles = await _authRepository.GetRolesByUser(user);

                var token = _tokenGeneratorService.GenerateToken(user, roles);

                return new Response<LoginResponse>(new LoginResponse(user.Id, user.UserName, user.Email, token, roles));
            }

            return new Response<LoginResponse>("Credenciais inválidas.");
        }

        public async Task<Response<ApplicationUserResponse>> AssociateRole(string userId, List<string> rolesList)
        {
            var user = await _authRepository.FindById(userId);

            if (user is null)
                return new Response<ApplicationUserResponse>("Usuário não encontrado.");

            var roles = await _authRepository.GetRolesByName(rolesList);

            if (roles is null)
                return new Response<ApplicationUserResponse>("Perfis não encontrados.");

            var currentRoles = await _authRepository.GetRolesByUser(user);

            var removeResult = await _authRepository.RemoveRoles(user, currentRoles);

            if (removeResult.Succeeded)
            {
                var result = await _authRepository.AssociateRoles(user, roles.Select(r => r.Name).ToList());

                if (result.Succeeded)
                {
                    return new Response<ApplicationUserResponse>(new ApplicationUserResponse(user.Id, user.UserName, user.Email, null, roles.Select(r => r.Name).ToList()));
                }

                return new Response<ApplicationUserResponse>(result.Errors.Select(e => e.Description).ToList());
            }

            return new Response<ApplicationUserResponse>(removeResult.Errors.Select(e => e.Description).ToList());
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
                return new Response<ApplicationUserResponse>(new ApplicationUserResponse(user.Id, user.UserName, user.Email, null, null));

            return new Response<ApplicationUserResponse>(result.Errors.Select(e => e.Description).ToList());
        }

        //MASTER ADMIN ACTIONS
        public async Task<Response<ApplicationUserResponse>> MasterAdminCreateFirstForCompany(CreateAccountRequest request)
        {
            try
            {
                var applicationUser = new ApplicationUser(request.Email, request.Username);
                applicationUser.AssignCompany(request.CompanyId.Value);

                var result = await _authRepository.CreateAccount(applicationUser, request.Password, true);

                if (result.Succeeded)
                {

                    var roleResult = await _authRepository.AssociateRole(applicationUser, "Admin");

                    _backgroundEmailJobWrapper.SendEmail(new EmailMessage { To = applicationUser.Email, Subject = "Bem-vindo! - Fixeon", Body = EmailDictionary.WelcomeEmail });

                    return new Response<ApplicationUserResponse>(new ApplicationUserResponse(
                        applicationUser.Id,
                        applicationUser.UserName,
                        applicationUser.Email,
                        null,
                        new List<string> { "Admin" }));
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
                response.Add(new ApplicationUserResponse(u.Id, u.UserName, u.Email, null, roles));
            }

            return new Response<List<ApplicationUserResponse>>(response);
        }
    }
}
