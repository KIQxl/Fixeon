using Fixeon.Auth.Infraestructure.Configuration;
using Fixeon.Auth.Infraestructure.Entities;
using Fixeon.Auth.Infraestructure.Interfaces;
using Fixeon.Shared.Core.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Fixeon.Auth.Infraestructure.Services
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenGeneratorService(IOptions<JwtSettings> jwtSettings, ITenantContextServices tenantContext)
        {
            this._jwtSettings = jwtSettings.Value;
        }

        public string GenerateToken(ApplicationUser user, List<string> roles)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim("id", user.Id));
            claims.Add(new Claim("username", user.UserName));
            claims.Add(new Claim("email", user.Email));
            claims.Add(new Claim("companyId", user.CompanyId.ToString()));

            if (user.OrganizationId != null)
            {
                claims.Add(new Claim("organizationId", user.OrganizationId.ToString()));
            }
            else
            {
                claims.Add(new Claim("organizationId", string.Empty));
            }

            if (roles != null && roles.Any())
            {
                var rolesJson = JsonSerializer.Serialize(roles);
                claims.Add(new Claim("roles", rolesJson, JsonClaimValueTypes.JsonArray));
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
