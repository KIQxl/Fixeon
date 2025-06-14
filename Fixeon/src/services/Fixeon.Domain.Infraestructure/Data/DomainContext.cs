using Fixeon.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fixeon.Domain.Infraestructure.Data
{
    public class DomainContext : DbContext
    {
        public DomainContext(DbContextOptions<DomainContext> opts) 
            : base (opts) { }

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

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DomainContext).Assembly);
        }
    }
}
