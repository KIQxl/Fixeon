using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Application.Services;
using Fixeon.Domain.Infraestructure.Data;
using Fixeon.Domain.Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fixeon.Domain.Infraestructure.Configuration
{
    public static class AddConfigurations
    {
        public static IServiceCollection RegisterDomainContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DomainContext>((serviceProvider, opts) =>
            {
                opts
                .UseSqlServer(configuration["ConnectionStrings:FixeonDefaultConnection"])
                .AddInterceptors(serviceProvider.GetRequiredService<TenantSaveChangesInterceptor>());
            });

            return services;
        }

        public static IServiceCollection RegisterDomainDI(this IServiceCollection services)
        {
            services.AddScoped<ITicketServices, TicketServices>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWOrk>();
            services.AddScoped<TenantSaveChangesInterceptor>();

            return services;
        }
    }
}
