using Fixeon.Auth.Application.Dtos.Requests;
using Fixeon.Auth.Application.Dtos.Responses;
using Fixeon.Auth.Application.Interfaces;
using Fixeon.Shared.Configuration;

namespace Fixeon.Auth.Application.Services
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly ITokenGeneratorService _tokenService;
        private readonly IBackgroundEmailJobWrapper _backgroundEmailJobWrapper;
        private readonly IUrlEncoder _urlEncoder;
        private readonly IIdentityServices _identityUserServices;

        public AuthenticationServices(ITokenGeneratorService tokenService, IBackgroundEmailJobWrapper backgroundEmailJobWrapper, IUrlEncoder urlEncoder, IIdentityServices identityUserServices)
        {
            _tokenService = tokenService;
            _backgroundEmailJobWrapper = backgroundEmailJobWrapper;
            _urlEncoder = urlEncoder;
            _identityUserServices = identityUserServices;
        }

        public async Task<Response<LoginResponse>> Login(LoginRequest request)
        {
            var emailExists = await _identityUserServices.FindUserByEmail(request.Email);

            if (!emailExists)
                return new Response<LoginResponse>("Usuário não encontrado.");

            var appUser = await _identityUserServices.Login(request.Email, request.Password);

            if (appUser.Errors.Any())
                return new Response<LoginResponse>(appUser.Errors);

            var token = _tokenService.GenerateToken(appUser);

            return new Response<LoginResponse>(new LoginResponse { Id = appUser.Id, Email = appUser.Email, Username = appUser.Username, Token = token, Roles = appUser.Roles });
        }

        public async Task<Response<LoginResponse>> CreateAccount(CreateAccountRequest request)
        {
            var emailExists = await _identityUserServices.FindUserByEmail(request.Email);

            if (emailExists)
                return new Response<LoginResponse>("Email já cadastrado na base.");

            var appUser = await _identityUserServices.CreateIdentityUser(request);

            if (appUser is null || appUser.Errors.Any())
                return new Response<LoginResponse>(appUser.Errors);

            var token = _tokenService.GenerateToken(appUser);

            _backgroundEmailJobWrapper.SendEmail(new Shared.Models.EmailMessage { To = appUser.Email, Subject = "Boas vindas", Body = EmailDictionary.WelcomeEmail });

            return new Response<LoginResponse>(new LoginResponse { Id = appUser.Id, Email = appUser.Email, Username = appUser.Username, Token = token, Roles = appUser.Roles });
        }

        public async Task<Response<bool>> CreateRole(string request)
        {
            var result = await _identityUserServices.CreateRole(request);

            if (result)
                return new Response<bool>(result);

            return new Response<bool>("Não foi possivel cadastrar o novo perfil.");
        }

        public async Task<Response<ApplicationUserResponse>> AssociateRole(string email, string role)
        {
            var result = await _identityUserServices.AssociateRole(email, role);

            if (result.Errors.Any())
                return new Response<ApplicationUserResponse>(result.Errors);

            return new Response<ApplicationUserResponse>(result);
        }

        public async Task<Response<ApplicationUserResponse>> GetUserByIdAsync(string userId)
        {
            var user = await _identityUserServices.GetuserById(userId);

            if (user is null || user.Errors.Any())
                return new Response<ApplicationUserResponse>(user.Errors);

            return new Response<ApplicationUserResponse>(user);
        }

        public async Task<Response<List<ApplicationUserResponse>>> GetAllUsersAsync()
        {
            var users = await _identityUserServices.GetAllUsers();

            if (users is null)
                return new Response<List<ApplicationUserResponse>>("Nenhum usuário encontrado.");

            return new Response<List<ApplicationUserResponse>>(users);
        }

        public async Task<Response<bool>> SendRecoveryPasswordLink(string email)
        {
            var token = await _identityUserServices.GenerateResetPasswordToken(email);

            if (token is null)
                return new Response<bool>("Não foi possivel gerar o código de recuperação.");

            var encodedToken = $"?recovery-token={_urlEncoder.Encode(token)}";

            _backgroundEmailJobWrapper.SendEmail(new Shared.Models.EmailMessage { To = email, Subject = "Recuperação de Senha - Fixeon", Body = EmailDictionary.ResetPasswordEmail.Replace("{{reset_link}}", encodedToken) });

            return new Response<bool>(true);
        }

        public async Task<Response<ApplicationUserResponse>> ResetPassword(ResetPasswordRequest request)
        {
            request.Token = _urlEncoder.Decode(request.Token);

            var result = await _identityUserServices.ResetPassword(request);

            if (!result.Errors.Any())
                return new Response<ApplicationUserResponse>(result);

            return new Response<ApplicationUserResponse>(result.Errors);
        }
    }
}
