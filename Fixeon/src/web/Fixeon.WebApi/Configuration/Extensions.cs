using Fixeon.Auth.Infraestructure.Configuration;
using Fixeon.Domain.Application.Dtos.Enums;
using Fixeon.Domain.Application.Dtos.Responses;
using Fixeon.Domain.Infraestructure.Configuration;
using Fixeon.Shared.Interfaces;
using Fixeon.Shared.Services;
using Fixeon.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
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
                .AddJsonOptions( options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            var jwtSection = configuration.GetSection("JwtSection");
            services.Configure<JwtSettings>(jwtSection);

            var jwtSettings = jwtSection.Get<JwtSettings>();

            services.RegisterIdentityConfigs(configuration)
                .RegisterDomainContext(configuration)
                .RegisterIdentityAuthentication(jwtSettings)
                .RegisterDI()
                .RegisterDomainDI()
                .RegisterBackgroundServices();

            return services;
        }

        public static IServiceCollection RegisterBackgroundServices(this IServiceCollection services)
        {
            services.AddHostedService<EmailBackgroundService>();

            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379"));
            services.AddSingleton<IEmailQueueServices, EmailQueueServices>();
            services.AddSingleton<IEmailServices, EmailService>();

            return services;
        }

        public static IApplicationBuilder RegisterApp (this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();

            app.UseAuthentication();
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
