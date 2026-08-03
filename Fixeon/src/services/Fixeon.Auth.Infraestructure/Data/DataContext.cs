using Fixeon.Auth.Infraestructure.Entities;
using Fixeon.Shared.Core.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fixeon.Auth.Infraestructure.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        private readonly ITenantContextServices _tenantContext;
        public Guid CurrentTenant => _tenantContext.TenantId;
        public List<string> CurrentRoles => _tenantContext.Roles;

        public bool IgnoreTenantInterceptor { get; set; } = false;
        public DataContext(DbContextOptions<DataContext> opts, ITenantContextServices tenantContext)
            : base(opts)
        {
            _tenantContext = tenantContext;
        }

        public DbSet<ApplicationUser> users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);


            builder.Entity<ApplicationUser>()
                .HasQueryFilter(u => (CurrentRoles != null && CurrentRoles.Contains("MasterAdmin")) || u.CompanyId == CurrentTenant);

            base.OnModelCreating(builder);
        }
    }
}
