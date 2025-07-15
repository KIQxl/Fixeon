using Fixeon.Auth.Application.Dtos.Responses;
using Fixeon.Auth.Application.Interfaces;
using Fixeon.Auth.Infraestructure.Configuration;
using Fixeon.Shared.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Fixeon.Auth.Infraestructure.Services
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly ITenantContext _tenantContext;

        public TokenGeneratorService(IOptions<JwtSettings> jwtSettings, ITenantContext tenantContext)
        {
            this._jwtSettings = jwtSettings.Value;
            _tenantContext = tenantContext;
        }

        public string GenerateToken(ApplicationUserResponse user)
        {
            var tenantId = _tenantContext.TenantId;

            var claims = new List<Claim>();

            claims.Add(new Claim("Id", user.Id));
            claims.Add(new Claim("Username", user.Username));
            claims.Add(new Claim("Email", user.Email));
            claims.Add(new Claim("CompanyId", tenantId.ToString()));

            if(user.Roles != null)
                foreach (var role in user.Roles)
                {
                    claims.Add(new Claim("Role", role));
                }

            var claimsIdentity = new ClaimsIdentity(claims);

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiresInHours),
                Subject = claimsIdentity,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }
    }
}
