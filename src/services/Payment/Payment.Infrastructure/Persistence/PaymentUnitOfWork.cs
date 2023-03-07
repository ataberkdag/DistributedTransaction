using Core.Application.Services;
using Core.Infrastructure.Persistence;
using Payment.Domain.Interfaces;

namespace Payment.Infrastructure.Persistence
{
    public class PaymentUnitOfWork : BaseUnitOfWork, IPaymentUnitOfWork
    {
        private readonly IPaymentTransactionRepository _paymentTransactions;
        public IPaymentTransactionRepository PaymentTransactions => _paymentTransactions;

        public PaymentUnitOfWork(PaymentDbContext context, IServiceProvider provider, IIntegrationEventBuilder eventBuilder) : base(context, provider, eventBuilder)
        {
            _paymentTransactions = (IPaymentTransactionRepository)provider.GetService(typeof(IPaymentTransactionRepository));
        }

    }
}
