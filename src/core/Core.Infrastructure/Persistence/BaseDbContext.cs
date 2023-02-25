using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Core.Infrastructure.Persistence
{
    public class BaseDbContext : DbContext
    {
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        public BaseDbContext(DbContextOptions opt) : base(opt)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
