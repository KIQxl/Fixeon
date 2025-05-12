using Fixeon.Auth.Application.Dtos;
using Fixeon.Auth.Application.Interfaces;

namespace Fixeon.Auth.Application.Services
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly IAuthRepository _rep;
        private readonly ITokenGeneratorService _tokenService;

        public AuthenticationServices(IAuthRepository services, ITokenGeneratorService tokenService)
        {
            _rep = services;
            _tokenService = tokenService;
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

            if (appUser is null)
                return new Response<LoginResponse>("Não foi possivel criar o usuário");

            var token = _tokenService.GenerateToken(appUser);

            return new Response<LoginResponse>(new LoginResponse { Id = appUser.Id, Email = appUser.Email, Username = appUser.Username, Token = token });
        }
    }
}
