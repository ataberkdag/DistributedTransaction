using Core.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Stock.Infrastructure.Persistence
{
    public class StockDbContext : BaseDbContext
    {
        public DbSet<Stock.Domain.Entities.Stock> Stocks { get; set; }
        public StockDbContext(DbContextOptions opt) : base(opt)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // TODO: Check
            base.OnModelCreating(modelBuilder);
        }
    }
}
