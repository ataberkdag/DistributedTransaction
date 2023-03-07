using Core.Infrastructure.Persistence;
using Payment.Domain.Entities;
using Payment.Domain.Interfaces;

namespace Payment.Infrastructure.Persistence
{
    public class PaymentTransactionRepository : GenericRepository<PaymentTransaction>, IPaymentTransactionRepository
    {
        public PaymentTransactionRepository(PaymentDbContext context) : base(context)
        {

        }
    }
}
