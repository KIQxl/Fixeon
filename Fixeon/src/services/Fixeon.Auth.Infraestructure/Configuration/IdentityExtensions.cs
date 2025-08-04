using Fixeon.Auth.Application.Dtos.Responses;
using Fixeon.Auth.Infraestructure.Data;
using Fixeon.Auth.Infraestructure.Entities;
using Fixeon.Auth.Infraestructure.Interfaces;
using Fixeon.Auth.Infraestructure.Repositories;
using Fixeon.Auth.Infraestructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace Fixeon.Auth.Infraestructure.Configuration
{
    public static class IdentityExtensions
    {
        public static IServiceCollection RegisterIdentityConfigs(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>((serviceProvider, opts) =>
            {
                opts.UseSqlServer(configuration["ConnectionStrings:FixeonDefaultConnection"])
                .AddInterceptors(serviceProvider.GetRequiredService<TenantSaveChangesInterceptor>());
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
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

               options.Events = new JwtBearerEvents
               {
                   OnChallenge = async context =>
                   {
                       context.HandleResponse();

                       context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                       context.Response.ContentType = "application/json";

                       var response = new Response<object>("Token inválido ou não fornecido");

                       await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                   },

                   OnForbidden = async context =>
                   {
                       context.Response.StatusCode = StatusCodes.Status403Forbidden;
                       context.Response.ContentType = "application/json";

                       var response = new Response<object>("Acesso negado: você não tem permissão para esta ação");

                       await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                   }
               };
           });

            return services;
        }

        public static IServiceCollection RegisterDI(this IServiceCollection services)
        {
            services.AddScoped<IIdentityRepository, AuthRepository>();
            services.AddScoped<IIdentityServices, IdentityServices>();
            services.AddScoped<ICompanyServices, CompanyServices>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
            services.AddScoped<IUrlEncoder, UrlEncoder>();
            services.AddScoped<TenantSaveChangesInterceptor>();

            return services;
        }
    }
}
