using Fixeon.Auth.Application.Interfaces;
using Fixeon.Auth.Application.Services;
using Fixeon.Auth.Infraestructure.Data;
using Fixeon.Auth.Infraestructure.Repositories;
using Fixeon.Auth.Infraestructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Fixeon.Auth.Infraestructure.Configuration
{
    public static class IdentityExtensions
    {
        public static IServiceCollection RegisterIdentityConfigs(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(opts => opts.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opts =>
            {
                opts.Password.RequireDigit = true;
                opts.Password.RequireLowercase = true;
                opts.Password.RequireUppercase = true;
                opts.Password.RequiredLength = 8;

                opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                opts.Lockout.MaxFailedAccessAttempts = 5;
            });

            return services;
        }

        public static IServiceCollection RegisterIdentityAuthentication(this IServiceCollection services, JwtSettings jwtSettings)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateIssuerSigningKey = true,

                   ValidIssuer = jwtSettings.Issuer,
                   ValidAudience = jwtSettings.Audience,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
               };
           });

            return services;
        }

        public static IServiceCollection RegisterDI(this IServiceCollection services)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
            services.AddTransient<IAuthenticationServices, AuthenticationServices>();
            services.AddScoped<IUrlEncoder, UrlEncoder>();

            return services;
        }
    }
}
