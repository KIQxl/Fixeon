using Fixeon.Auth.Application.Interfaces;
using Fixeon.Auth.Infraestructure.Configuration;
using Fixeon.Domain.Application.Dtos.Enums;
using Fixeon.Domain.Application.Dtos.Responses;
using Fixeon.Domain.Infraestructure.Configuration;
using Fixeon.Shared.Configuration;
using Fixeon.Shared.Interfaces;
using Fixeon.Shared.Services;
using Fixeon.WebApi.Services;
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
            services.AddSwaggerGen();

            var jwtSection = configuration.GetSection("JwtSection");
            services.Configure<JwtSettings>(jwtSection);

            var jwtSettings = jwtSection.Get<JwtSettings>();

            var smtpSection = configuration.GetSection("SmtpSettings");
            services.Configure<SmtpSettings>(smtpSection);

            var smtpSettings = smtpSection.Get<SmtpSettings>();

            services.RegisterIdentityConfigs(configuration)
                .RegisterDomainContext(configuration)
                .RegisterIdentityAuthentication(jwtSettings)
                .RegisterDI()
                .RegisterDomainDI()
                .RegisterBackgroundServices(configuration);

            return services;
        }

        public static IServiceCollection RegisterBackgroundServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEmailServices, EmailService>();
            services.AddScoped<IBackgroundEmailJobWrapper, HangfireWrapper>();

            services.AddHangfire(config => config.UseRedisStorage("localhost:6379"));
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
