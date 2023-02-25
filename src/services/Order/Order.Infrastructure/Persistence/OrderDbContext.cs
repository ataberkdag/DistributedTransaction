using Core.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Order.Domain.Entities;
using System.Reflection;

namespace Order.Infrastructure.Persistence
{
    public class OrderDbContext : BaseDbContext
    {
        public DbSet<Order.Domain.Entities.Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public OrderDbContext(DbContextOptions opt) : base(opt)
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
