using Fixeon.Auth.Infraestructure.Entities;
using Fixeon.Shared.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fixeon.Auth.Infraestructure.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        private readonly ITenantContext _tenantContext;
        public DataContext(DbContextOptions<DataContext> opts, ITenantContext tenantContext)
            : base(opts)
        {
            _tenantContext = tenantContext;
        }

        public DbSet<ApplicationUser> users { get; set; }
        public DbSet<Company> companies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

            var _currentTenant = _tenantContext.TenantId;

            builder.Entity<ApplicationUser>()
                .HasQueryFilter(u => u.CompanyId == _currentTenant);

            base.OnModelCreating(builder);
        }
    }
}
