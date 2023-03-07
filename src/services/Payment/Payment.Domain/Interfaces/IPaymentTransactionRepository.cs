using Core.Domain.Interfaces;
using Payment.Domain.Entities;

namespace Payment.Domain.Interfaces
{
    public interface IPaymentTransactionRepository : IGenericRepository<PaymentTransaction>
    {
    }
}
