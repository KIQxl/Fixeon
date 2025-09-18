using Fixeon.Auth.Infraestructure.Configuration;
using Fixeon.Auth.Infraestructure.Data;
using Fixeon.Auth.Infraestructure.Entities;
using Fixeon.Auth.Infraestructure.Interfaces;
using Fixeon.Auth.Infraestructure.Repositories;
using Fixeon.Auth.Infraestructure.Services;
using Fixeon.Domain.Application.Contracts;
using Fixeon.Domain.Application.Dtos.Enums;
using Fixeon.Domain.Application.Dtos.Responses;
using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Application.Services;
using Fixeon.Domain.Infraestructure.Data;
using Fixeon.Domain.Infraestructure.Repositories;
using Fixeon.Shared.Configuration;
using Fixeon.Shared.Core.Interfaces;
using Fixeon.Shared.Models;
using Fixeon.Shared.Services;
using Fixeon.WebApi.Middlewares;
using Hangfire;
using Hangfire.Redis.StackExchange;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fixeon.WebApi.Configuration
{
    public static class Extensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "Fixeon API", Version = "v1" });

                // Configuração do JWT no Swagger
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Insira o token JWT no formato: Bearer {seu token}"
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var jwtSection = configuration.GetSection("JwtSection");
            services.Configure<JwtSettings>(jwtSection);

            var jwtSettings = jwtSection.Get<JwtSettings>();

            services.RegisterIdentityAuthDataContextConfigs(configuration)
                .RegisterAuthContextDI()
                .RegisterTicketDataContext(configuration)
                .RegisterTicketContextDI()
                .RegisterIdentityAuthentication(jwtSettings)
                .RegisterBackgroundServices(configuration);

            services.AddHttpContextAccessor();
            services.AddScoped<ITenantContextServices, TenantContextServices>();
            services.AddScoped<IOrganizationResolver, OrganizationResolver>();
            services.AddScoped<ICompanyResolver, CompanyResolver>();

            var smtpSection = configuration.GetSection("SmtpSettings");
            services.Configure<SmtpSettings>(smtpSection);

            var smtpSettings = smtpSection.Get<SmtpSettings>();

            var storageProvider = configuration.GetValue<string>("StorageProvider");

            if (storageProvider == "MinIO")
            {
                var minIOStorageSection = configuration.GetSection("MinIOStorage");
                services.Configure<StorageSettings>(minIOStorageSection);
            }
            else
            {
                var S3StorageSection = configuration.GetSection("S3Storage");
                services.Configure<StorageSettings>(S3StorageSection);
            }

            services.AddScoped<StorageClientFactory>();
            services.AddScoped<IStorageServices>(sp =>
            {
                var factory = sp.GetRequiredService<StorageClientFactory>();
                return factory.GetService();
            });

            return services;
        }

        public static IServiceCollection RegisterBackgroundServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEmailServices, EmailService>();
            services.AddScoped<IBackgroundEmailJobWrapper, HangfireWrapper>();

            var redisConnection = configuration.GetConnectionString("FixeonRedis")
                          ?? throw new InvalidOperationException("Redis connection string is missing");

            services.AddHangfire(config => config.UseRedisStorage(redisConnection));
            services.AddHangfireServer(opts =>
            {
                opts.Queues = new[] { "email", "default" };
            });

            return services;
        }

        public static IApplicationBuilder RegisterApp(this IApplicationBuilder app)
        {
            app.UseHangfireDashboard("/hangfire");

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseMiddleware<TenantMiddleware>();
            app.UseAuthorization();

            return app;
        }

        public static IActionResult ReturnResponseWithStatusCode<T>(this ControllerBase controller, Response<T> response)
        {
            return (response.ErrorType switch
            {
                EErrorType.BadRequest => controller.BadRequest(response),
                EErrorType.NotFound => controller.NotFound(response),
                EErrorType.ServerError => controller.StatusCode(StatusCodes.Status500InternalServerError, response),
                _ => controller.StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado.")
            });
        }

        public static IServiceCollection RegisterTicketDataContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DomainContext>((serviceProvider, opts) =>
            {
                opts
                .UseSqlServer(configuration["ConnectionStrings:FixeonDefaultConnection"])
                .AddInterceptors(serviceProvider.GetRequiredService<Fixeon.Domain.Infraestructure.Data.TenantSaveChangesInterceptor>());
            });

            return services;
        }

        public static IServiceCollection RegisterTicketContextDI(this IServiceCollection services)
        {
            services.AddScoped<ITicketServices, TicketServices>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWOrk>();
            services.AddScoped<Fixeon.Domain.Infraestructure.Data.TenantSaveChangesInterceptor>();
            services.AddScoped<ICompanyServices, CompanyServices>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IOrganizationServices, OrganizationServices>();
            services.AddScoped<ITicketNotificationServices, TicketNotificationServices>();

            return services;
        }

        public static IServiceCollection RegisterIdentityAuthDataContextConfigs(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>((serviceProvider, opts) =>
            {
                opts.UseSqlServer(configuration["ConnectionStrings:FixeonDefaultConnection"])
                .AddInterceptors(serviceProvider.GetRequiredService<Fixeon.Auth.Infraestructure.Data.TenantSaveChangesInterceptor>());
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

                opts.User.AllowedUserNameCharacters = null;
            });

            return services;
        }

        public static IServiceCollection RegisterIdentityAuthentication(this IServiceCollection services, JwtSettings jwtSettings)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

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

            services.AddAuthorization(opts =>
            {
                AuthorizationPolicies.AddPolicies(opts);
            });

            return services;
        }

        public static IServiceCollection RegisterAuthContextDI(this IServiceCollection services)
        {
            services.AddScoped<IIdentityRepository, AuthRepository>();
            services.AddScoped<IIdentityServices, IdentityServices>();
            services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
            services.AddScoped<IUrlEncoder, UrlEncoder>();
            services.AddScoped<Fixeon.Auth.Infraestructure.Data.TenantSaveChangesInterceptor>();

            return services;
        }
    }
}
