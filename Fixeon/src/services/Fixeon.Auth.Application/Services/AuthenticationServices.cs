using Fixeon.Auth.Application.Dtos.Requests;
using Fixeon.Auth.Application.Dtos.Responses;
using Fixeon.Auth.Application.Interfaces;
using Fixeon.Shared.Configuration;

namespace Fixeon.Auth.Application.Services
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly IAuthRepository _rep;
        private readonly ITokenGeneratorService _tokenService;
        private readonly IBackgroundEmailJobWrapper _backgroundEmailJobWrapper;
        private readonly IUrlEncoder _urlEncoder;

        public AuthenticationServices(IAuthRepository services, ITokenGeneratorService tokenService, IBackgroundEmailJobWrapper backgroundEmailJobWrapper, IUrlEncoder urlEncoder)
        {
            _rep = services;
            _tokenService = tokenService;
            _backgroundEmailJobWrapper = backgroundEmailJobWrapper;
            _urlEncoder = urlEncoder;
        }

        public async Task<Response<LoginResponse>> Login(LoginRequest request)
        {
            var emailExists = await _rep.FindByEmail(request.Email);

            if (!emailExists)
                return new Response<LoginResponse>("Usuário não encontrado");

            var appUser = await _rep.Login(request.Email, request.Password);

            if (appUser is null)
                return new Response<LoginResponse>("Credenciais inválidas");

            var token = _tokenService.GenerateToken(appUser);

            return new Response<LoginResponse>(new LoginResponse { Id = appUser.Id, Email = appUser.Email, Username = appUser.Username, Token = token, Roles = appUser.Roles });
        }

        public async Task<Response<LoginResponse>> CreateAccount(CreateAccountRequest request)
        {
            var emailExists = await _rep.FindByEmail(request.Email);

            if (emailExists)
                return new Response<LoginResponse>("Email já cadastrado na base");

            var appUser = await _rep.CreateAccount(request);

            if (appUser is null || appUser.Errors.Any())
                return new Response<LoginResponse>(appUser.Errors);

            var token = _tokenService.GenerateToken(appUser);

            _backgroundEmailJobWrapper.SendEmail(new Shared.Models.EmailMessage { To = appUser.Email, Subject = "Boas vindas", Body = EmailDictionary.WelcomeEmail });

            return new Response<LoginResponse>(new LoginResponse { Id = appUser.Id, Email = appUser.Email, Username = appUser.Username, Token = token, Roles = appUser.Roles });
        }

        public async Task<Response<bool>> CreateRole(string request)
        {
            var result = await _rep.CreateRole(request);

            if (result)
                return new Response<bool>(result);

            return new Response<bool>("Não foi possivel cadastrar o novo perfil");
        }

        public async Task<Response<LoginResponse>> AssociateRole(string userId, string role)
        {
            var result = await _rep.AssociateRole(userId, role);

            if (result.Errors.Any())
                return new Response<LoginResponse>(result.Errors);

            return new Response<LoginResponse>(new LoginResponse { Id = result.Id, Email = result.Email, Username = result.Username, Token = string.Empty, Roles = result.Roles });
        }

        public async Task<Response<ApplicationUserResponse>> GetUserEmailAsync(string userId)
        {
            var user = await _rep.GetUser(userId);

            if (user is null || user.Errors.Any())
                return new Response<ApplicationUserResponse>(user.Errors);

            return new Response<ApplicationUserResponse>(user);
        }

        public async Task<Response<List<ApplicationUserResponse>>> GetAllUsersAsync()
        {
            var users = await _rep.GetAllUsers();

            if (users is null)
                return new Response<List<ApplicationUserResponse>>("Nenhum usuário encontrado");

            return new Response<List<ApplicationUserResponse>>(users);
        }

        public async Task<Response<bool>> SendRecoveryPasswordLink(string email)
        {
            var user = await _rep.GetUser(email);

            if(user is null)
                return new Response<bool>("Usuário não encontrado");

            var token = await _rep.GenerateResetPasswordToken(email);
            var encodedToken = $"?recovery-token={_urlEncoder.Encode(token)}";

            _backgroundEmailJobWrapper.SendEmail(new Shared.Models.EmailMessage { To = user.Email, Subject = "Recuperação de Senha - Fixeon", Body = EmailDictionary.ResetPasswordEmail.Replace("{{reset_link}}", encodedToken) });

            return new Response<bool>(true);
        }

        public async Task<Response<ApplicationUserResponse>> ResetPassword(ResetPasswordRequest request)
        {
            request.Token = _urlEncoder.Decode(request.Token);

            var result = await _rep.ResetPassword(request);

            if (!result.Errors.Any())
                return new Response<ApplicationUserResponse>(result);

            return new Response<ApplicationUserResponse>(result.Errors);
        }


    }
}
