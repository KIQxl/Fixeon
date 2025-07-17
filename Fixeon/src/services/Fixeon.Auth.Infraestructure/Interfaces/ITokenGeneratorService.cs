using Fixeon.Auth.Application.Dtos.Responses;
using Fixeon.Auth.Infraestructure.Dtos.Responses;

namespace Fixeon.Auth.Infraestructure.Interfaces
{
    public interface ITokenGeneratorService
    {
        public string GenerateToken(ApplicationUserResponse user);
    }
}
