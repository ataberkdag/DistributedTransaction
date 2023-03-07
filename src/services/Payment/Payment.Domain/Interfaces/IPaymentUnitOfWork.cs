using Core.Domain.Interfaces;

namespace Payment.Domain.Interfaces
{
    public interface IPaymentUnitOfWork : IBaseUnitOfWork
    {
        IPaymentTransactionRepository PaymentTransactions { get; }
    }
}
