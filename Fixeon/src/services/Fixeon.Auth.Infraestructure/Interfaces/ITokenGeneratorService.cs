using Fixeon.Auth.Infraestructure.Entities;

namespace Fixeon.Auth.Infraestructure.Interfaces
{
    public interface ITokenGeneratorService
    {
        public string GenerateToken(ApplicationUser user, List<string> roles);
    }
}
