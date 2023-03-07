using Core.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Payment.Domain.Entities;
using System.Reflection;

namespace Payment.Infrastructure.Persistence
{
    public class PaymentDbContext : BaseDbContext
    {
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        public PaymentDbContext(DbContextOptions opt) : base(opt)
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
