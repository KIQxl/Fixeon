using Fixeon.Auth.Application.Dtos.Responses;

namespace Fixeon.Auth.Application.Interfaces
{
    public interface ITokenGeneratorService
    {
        public string GenerateToken(ApplicationUserResponse user);
    }
}
