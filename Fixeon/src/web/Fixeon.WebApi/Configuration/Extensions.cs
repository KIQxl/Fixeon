using Fixeon.Auth.Infraestructure.Configuration;
using Fixeon.Domain.Application.Dtos.Enums;
using Fixeon.Domain.Application.Dtos.Responses;
using Fixeon.Domain.Infraestructure.Configuration;
using Fixeon.Shared.Configuration;
using Fixeon.Shared.Core.Interfaces;
using Fixeon.Shared.Models;
using Fixeon.Shared.Services;
using Fixeon.WebApi.Middlewares;
using Hangfire;
using Hangfire.Redis.StackExchange;
using Microsoft.AspNetCore.Mvc;
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

            services.RegisterIdentityConfigs(configuration)
                .RegisterDomainContext(configuration)
                .RegisterIdentityAuthentication(jwtSettings)
                .RegisterDI()
                .RegisterDomainDI()
                .RegisterBackgroundServices(configuration);

            services.AddHttpContextAccessor();
            services.AddScoped<ITenantContext, TenantContext>();

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

            services.AddSingleton<StorageClientFactory>();
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
    }
}
