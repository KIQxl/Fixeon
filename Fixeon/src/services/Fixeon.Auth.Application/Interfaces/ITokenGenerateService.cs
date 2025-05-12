using Fixeon.Auth.Application.Dtos;

namespace Fixeon.Auth.Application.Interfaces
{
    public interface ITokenGeneratorService
    {
        public string GenerateToken(ApplicationUser user);
    }
}
