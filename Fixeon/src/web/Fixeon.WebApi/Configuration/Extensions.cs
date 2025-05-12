using Fixeon.Auth.Infraestructure.Configuration;
using Fixeon.Domain.Application.Dtos.Enums;
using Fixeon.Domain.Application.Dtos.Responses;
using Fixeon.Domain.Infraestructure.Configuration;
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
                .RegisterDomainDI();

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
