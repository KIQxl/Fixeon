using Fixeon.Auth.Application.Dtos;
using Fixeon.Auth.Application.Interfaces;
using Fixeon.Shared.Interfaces;

namespace Fixeon.Auth.Application.Services
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly IAuthRepository _rep;
        private readonly ITokenGeneratorService _tokenService;
        //private readonly IEmailQueueServices _emailQueue;
        private readonly IBackgroundEmailJobWrapper _backgroundEmailJobWrapper;

        public AuthenticationServices(IAuthRepository services, ITokenGeneratorService tokenService, IEmailQueueServices emailQueue, IBackgroundEmailJobWrapper backgroundEmailJobWrapper)
        {
            _rep = services;
            _tokenService = tokenService;
            //_emailQueue = emailQueue;
            _backgroundEmailJobWrapper = backgroundEmailJobWrapper;
        }

        public async Task<Response<LoginResponse>> Login(LoginRequest request)
        {
            var emailExists = await _rep.FindByEmail(request.Email);

            if (!emailExists)
                return new Response<LoginResponse>("Usuário não encontrado");

            var appUser = await _rep.Login(request.Email, request.Password);

            if(appUser is null)
                return new Response<LoginResponse>("Credenciais inválidas");

            var token = _tokenService.GenerateToken(appUser);

            return new Response<LoginResponse>(new LoginResponse { Id = appUser.Id, Email = appUser.Email, Username = appUser.Username, Token = token});
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

            //await _emailQueue.EnqueueEmailAsync(new Shared.Models.EmailMessage { To = appUser.Email, Subject = "Boas vindas", Body = "Cadastro completo."});
            _backgroundEmailJobWrapper.SendWelcomeEmail(new Shared.Models.EmailMessage { To = appUser.Email, Subject = "Boas vindas", Body = "Cadastro completo." });

            return new Response<LoginResponse>(new LoginResponse { Id = appUser.Id, Email = appUser.Email, Username = appUser.Username, Token = token });
        }
    }
}
