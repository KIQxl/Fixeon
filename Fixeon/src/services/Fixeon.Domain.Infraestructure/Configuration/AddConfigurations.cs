using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Application.Services;
using Fixeon.Domain.Infraestructure.Data;
using Fixeon.Domain.Infraestructure.Repositories;
using Fixeon.Domain.Infraestructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fixeon.Domain.Infraestructure.Configuration
{
    public static class AddConfigurations
    {
        public static IServiceCollection RegisterDomainContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DomainContext>(opts => opts.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]));

            return services;
        }

        public static IServiceCollection RegisterDomainDI(this IServiceCollection services)
        {
            services.AddScoped<ITicketServices, TicketServices>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IFileServices, FileServices>();

            return services;
        }
    }
}
