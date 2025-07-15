using Fixeon.Domain.Core.Entities;
using Fixeon.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fixeon.Domain.Infraestructure.Data
{
    public class DomainContext : DbContext
    {
        private readonly ITenantContext _tenantContext;
        public DomainContext(DbContextOptions<DomainContext> opts, ITenantContext tenantContext)
            : base(opts)
        {
            _tenantContext = tenantContext;
        }

        public DbSet<Ticket> tickets { get; set; }
        public DbSet<Interaction> interactions { get; set; }
        public DbSet<Attachment> attachments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(
                    e => e.GetProperties()
                    .Where(
                        p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            var _currentTenant = _tenantContext.TenantId;

            modelBuilder.Entity<Ticket>()
                .HasQueryFilter(t => t.CompanyId == _currentTenant);

            modelBuilder.Entity<Interaction>()
                .HasQueryFilter(i => i.Ticket.CompanyId == _currentTenant);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DomainContext).Assembly);
        }
    }
}
