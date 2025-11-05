using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Entities;
using Fixeon.Shared.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fixeon.Domain.Infraestructure.Data
{
    public class DomainContext : DbContext
    {
        private readonly ITenantContextServices _tenantContext;
        public Guid _currentTenant => _tenantContext.TenantId;
        public DomainContext(DbContextOptions<DomainContext> opts, ITenantContextServices tenantContext)
            : base(opts)
        {
            _tenantContext = tenantContext;
        }

        public DbSet<Ticket> tickets { get; set; }
        public DbSet<Interaction> interactions { get; set; }
        public DbSet<Attachment> attachments { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Departament> departaments { get; set; }
        public DbSet<Company> companies { get; set; }
        public DbSet<Organization> organizations { get; set; }
        public DbSet<OrganizationsSLA> organizationsSLAs { get; set; }
        public DbSet<Tag> tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(
                    e => e.GetProperties()
                    .Where(
                        p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.Entity<Ticket>()
                .HasQueryFilter(t => t.CompanyId == _currentTenant);

            modelBuilder.Entity<Interaction>()
                .HasQueryFilter(i => i.Ticket.CompanyId == _currentTenant);

            modelBuilder.Entity<Organization>()
                .HasQueryFilter(u => u.CompanyId == _currentTenant);

            modelBuilder.Entity<Tag>()
                .HasQueryFilter(t => t.CompanyId == _currentTenant);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DomainContext).Assembly);
        }
    }
}
