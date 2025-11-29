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
        private readonly IOrganizationResolver _organizationResolver;
        private readonly IStorageServices _storageServices;

        public IdentityServices(IIdentityRepository authRepository, IBackgroundEmailJobWrapper backgroundEmailJobWrapper, IUrlEncoder urlEncoder, ITokenGeneratorService tokenGeneratorService, IOrganizationResolver organizationResolver, IStorageServices storageServices)
        {
            _authRepository = authRepository;
            _backgroundEmailJobWrapper = backgroundEmailJobWrapper;
            _urlEncoder = urlEncoder;
            _tokenGeneratorService = tokenGeneratorService;
            _organizationResolver = organizationResolver;
            _storageServices = storageServices;
        }

        // GET
        public async Task<Response<List<ApplicationUserResponse>>> GetAllUsers(Guid? id, string? email, Guid? organization, string? username)
        {
            var users = await _authRepository.GetAllUsers(false, id, email, organization, username);

            if (users is null)
                return new Response<List<ApplicationUserResponse>>("Nenhum usuário encontrado.");

            var userIds = users.Select(u => u.Id).ToList();
            var rolesDictionary = await _authRepository.GetRolesForUsers(userIds);

            var response = new List<ApplicationUserResponse>();

            var orgIds = users
                            .Where(u => u.OrganizationId.HasValue)
                            .Select(u => u.OrganizationId.Value)
                            .Distinct()
                            .ToList();

            var organizations = await _organizationResolver.GetOrganizations(orgIds);
            var orgDict = organizations.ToDictionary(o => o.OrganizationId, o => o);

            var responseTasks = users.Select(async u =>
            {
                var userRoles = rolesDictionary.GetValueOrDefault(u.Id.ToString(), new List<string>());

                // b. Pegar a organização do dicionário (em memória).
                UserOrganizationResponse? organizationResponse = null;
                if (u.OrganizationId.HasValue && orgDict.TryGetValue(u.OrganizationId.Value, out var org))
                {
                    organizationResponse = new UserOrganizationResponse
                    {
                        OrganizationId = org.OrganizationId,
                        OrganizationName = org.OrganizationName
                    };
                }

                string profilePictureUrl = null;
                if (!string.IsNullOrEmpty(u.ProfilePictureUrl))
                {
                    profilePictureUrl = await GetPresignedUrl("users/profile_pictures", u.ProfilePictureUrl);
                }

                return new ApplicationUserResponse(
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.PhoneNumber,
                    u.JobTitle,
                    profilePictureUrl,
                    organizationResponse,
                    userRoles
                );
            });

            var responseList = (await Task.WhenAll(responseTasks)).ToList();

            return new Response<List<ApplicationUserResponse>>(responseList);
        }

        public async Task<Response<ApplicationUserResponse>> GetuserById(string id)
        {
            var user = await _authRepository.FindById(id);

            if (user is null)
                return new Response<ApplicationUserResponse>("Usuário não encontrado.");

            var roles = await _authRepository.GetRolesByUser(user);

            UserOrganizationResponse organizationResponse = null;
            if (user.OrganizationId.HasValue)
            {
                var org = await _organizationResolver.GetOrganization(user.OrganizationId.Value);

                organizationResponse = new UserOrganizationResponse
                {
                    OrganizationId = org.OrganizationId,
                    OrganizationName = org.OrganizationName
                };
            }

            var urlImage = await GetPresignedUrl($"users/profile_pictures", user.ProfilePictureUrl);

            return new Response<ApplicationUserResponse>(new ApplicationUserResponse(user.Id, user.UserName, user.Email, user.PhoneNumber, user.JobTitle, urlImage, organizationResponse, roles));
        }

        public async Task<Response<List<ApplicationUserResponse>>> GetUserByRoleName(string role)
        {
            var users = await _authRepository.GetUsersByRoleName(role);

            if (users is null)
                return new Response<List<ApplicationUserResponse>>("Nenhum usuário encontrado.");

            var usersId = users.Select(u => u.Id).ToList();
            var roles = await _authRepository.GetRolesForUsers(usersId);

            var orgsId = users
                            .Where(u => u.OrganizationId.HasValue)
                            .Select(u => u.OrganizationId.Value)
                            .Distinct()
                            .ToList();

            var orgs = await _organizationResolver.GetOrganizations(orgsId);

            var orgsDict = orgs.ToDictionary(o => o.OrganizationId, o => o);

            var responseTasks = users.Select(async u =>
            {
                var userRoles = roles.GetValueOrDefault(u.Id.ToString(), new List<string>());

                UserOrganizationResponse? userOrg = null;
                if (u.OrganizationId.HasValue && orgsDict.TryGetValue(u.OrganizationId.Value, out var org))
                    userOrg = new UserOrganizationResponse
                    {
                        OrganizationId = org.OrganizationId,
                        OrganizationName = org.OrganizationName
                    };

                string profilePicture = await GetPresignedUrl("users/profile_pictures", u.ProfilePictureUrl);

                return new ApplicationUserResponse(u.Id, u.UserName, u.Email, u.PhoneNumber, u.JobTitle, profilePicture, userOrg, userRoles);
            });

            var response = (await Task.WhenAll(responseTasks)).ToList();

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

        public async Task<Response<List<ApplicationUserResponse>>> GetUsersByOrganiztionId(Guid organizationId)
        {
            var users = await _authRepository.GetUsersByOrganization(organizationId);

            if (users is null)
                return new Response<List<ApplicationUserResponse>>("Nenhum usuário encontrado.");

            var response = new List<ApplicationUserResponse>();

            foreach (var u in users)
            {
                var roles = await _authRepository.GetRolesByUser(u);
                var org = await _organizationResolver.GetOrganization(u.OrganizationId.Value);

                UserOrganizationResponse organizationResponse = new UserOrganizationResponse
                {
                    OrganizationId = org.OrganizationId,
                    OrganizationName = org.OrganizationName
                };

                response.Add(new ApplicationUserResponse(u.Id, u.UserName, u.Email, u.PhoneNumber, u.JobTitle, u.ProfilePictureUrl, organizationResponse, roles));
            }

            return new Response<List<ApplicationUserResponse>>(response);
        }

        // CREATE
        public async Task<Response<bool>> CreateIdentityUser(CreateAccountRequest request)
        {
            try
            {
                var user = new ApplicationUser(request.Email, request.Username, request.PhoneNumber, request.JobTitle, request.ProfilePictureUrl?.FileName);

                if (request.OrganizationId.HasValue)
                    user.AssignOrganization(request.OrganizationId.Value);

                var result = await _authRepository.CreateAccount(user, request.Password, false);

                if (result.Succeeded)
                {
                    if(request.ProfilePictureUrl != null)
                        await SaveFile(request.ProfilePictureUrl);

                    if (request.Roles != null && request.Roles.Any())
                    {
                        var roles = await _authRepository.GetRolesByName(request.Roles);

                        if (roles.Any())
                            await _authRepository.AssociateRoles(user, roles.Select(r => r.Name).ToList());
                    }

                    _backgroundEmailJobWrapper.SendEmail(new EmailMessage { To = user.Email, Subject = "Bem-vindo! - Fixeon", Body = EmailDictionary.WelcomeEmail });

                    return new Response<bool>(true);
                }

                return new Response<bool>(result.Errors.Select(e => e.Description).ToList());
            }
            catch (Exception ex)
            {
                return new Response<bool>(new List<string> { "Ocorreu um erro.", ex.InnerException?.Message ?? ex.Message });
            }
        }

        public async Task<Response<bool>> UpdateApplicationUser(UpdateApplicationUserRequest request)
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
                        return new Response<bool>(true);

                    return new Response<bool>(result.Errors
                        .Select(e => e.Description)
                        .ToList());
                }

                return new Response<bool>("Usuário não encontrado.");

            }
            catch (Exception ex)
            {
                return new Response<bool>(ex.Message);
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
                    return new Response<ApplicationUserResponse>(new ApplicationUserResponse(user.Id, user.UserName, user.Email, user.PhoneNumber, user.JobTitle, user.ProfilePictureUrl, null, roles.Select(r => r.Name).ToList()));
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
                return new Response<ApplicationUserResponse>(new ApplicationUserResponse(user.Id, user.UserName, user.Email, user.PhoneNumber, user.JobTitle, user.ProfilePictureUrl, null, null));

            return new Response<ApplicationUserResponse>(result.Errors.Select(e => e.Description).ToList());
        }

        //MASTER ADMIN ACTIONS
        public async Task<Response<bool>> MasterAdminCreateFirstForCompany(CreateAccountRequest request)
        {
            try
            {
                var applicationUser = new ApplicationUser(request.Email, request.Username, request.PhoneNumber, request.JobTitle, request.ProfilePictureUrl?.FileName);
                applicationUser.AssignCompany(request.CompanyId.Value);

                var result = await _authRepository.CreateAccount(applicationUser, request.Password, true);

                if (result.Succeeded)
                {
                    if(request.ProfilePictureUrl != null)
                        await SaveFile(request.ProfilePictureUrl);

                    var roleResult = await _authRepository.AssociateRole(applicationUser, "Admin");

                    _backgroundEmailJobWrapper.SendEmail(new EmailMessage { To = applicationUser.Email, Subject = "Bem-vindo! - Fixeon", Body = EmailDictionary.WelcomeEmail });

                    return new Response<bool>(true);
                }

                return new Response<bool>(result.Errors.Select(e => e.Description).ToList());
            }
            catch (Exception ex)
            {
                return new Response<bool>(new List<string> { "Ocorreu um erro.", ex.InnerException?.Message ?? ex.Message });
            }
        }

        public async Task<Response<bool>> CreateMasterAdmin(CreateAccountRequest request)
        {
            try
            {
                var applicationUser = new ApplicationUser(request.Email, request.Username, request.PhoneNumber, request.JobTitle, request.ProfilePictureUrl?.FileName);

                var result = await _authRepository.CreateAccount(applicationUser, request.Password, true);

                if (result.Succeeded)
                {
                    await SaveFile(request.ProfilePictureUrl);
                    var roleResult = await _authRepository.AssociateRole(applicationUser, "MasterAdmin");

                    //_backgroundEmailJobWrapper.SendEmail(new EmailMessage { To = applicationUser.Email, Subject = "Bem-vindo! - Fixeon", Body = EmailDictionary.WelcomeEmail });

                    return new Response<bool>(true);
                }

                return new Response<bool>(result.Errors.Select(e => e.Description).ToList());
            }
            catch (Exception ex)
            {
                return new Response<bool>(new List<string> { "Ocorreu um erro.", ex.InnerException?.Message ?? ex.Message });
            }
        }

        public async Task<Response<List<ApplicationUserResponse>>> MasterAdminGetAllUsers()
        {
            var users = await _authRepository.GetAllUsers(true, null, null, null, null);

            if (users is null)
                return new Response<List<ApplicationUserResponse>>("Nenhum usuário encontrado.");

            var userIds = users.Select(u => u.Id).ToList();

            var rolesDictionary = await _authRepository.GetRolesForUsers(userIds);

            var response = new List<ApplicationUserResponse>();

            var responseTasks = users.Select(async u =>
            {
                // a. Pega os papéis do dicionário (operação em memória).
                var userRoles = rolesDictionary.GetValueOrDefault<string, List<string>>(u.Id, new List<string>());

                // b. Gera a URL da foto de perfil de forma assíncrona.
                string profilePictureUrl = null;
                if (!string.IsNullOrEmpty(u.ProfilePictureUrl))
                    profilePictureUrl = await GetPresignedUrl("users/profile_pictures", u.ProfilePictureUrl);

                // c. Retorna o objeto de resposta construído.
                return new ApplicationUserResponse(
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.PhoneNumber,
                    u.JobTitle,
                    profilePictureUrl,
                    null,
                    userRoles
                );
            });

            var responseList = (await Task.WhenAll(responseTasks)).ToList();

            return new Response<List<ApplicationUserResponse>>(responseList);
        }

        private async Task<string> GetPresignedUrl(string path, string filename)
        {
            var presignedUrl = await _storageServices.GetPresignedUrl(path, filename);

            return presignedUrl;
        }

        private async Task SaveFile(FormFileAdapterDto file)
        {
            try
            {
                await _storageServices.UploadFile("users/profile_pictures", file.FileName, file.ContentType, file.Content);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao salvar anexos." + ex.Message);
            }
        }
    }
}
