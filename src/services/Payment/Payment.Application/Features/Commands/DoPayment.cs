using Core.Application.Common;
using MediatR;
using Payment.Domain.Entities;
using Payment.Domain.Interfaces;

namespace Payment.Application.Features.Commands
{
    public static class DoPayment
    {
        public class Command : IRequest<BaseResult>
        {
            public Guid UserId { get; set; }
            public Guid CorrelationId { get; set; }
            public decimal TotalAmount { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, BaseResult>
        {
            private readonly IPaymentUnitOfWork _uow;
            public CommandHandler(IPaymentUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var payment = PaymentTransaction.Create(request.CorrelationId, request.UserId, request.TotalAmount);

                // TODO: Rule Engine
                if (request.TotalAmount > 200)
                    payment.FailedPayment(BusinessExceptionCodes.ExceedingTotalLimit.GetHashCode().ToString(), BusinessExceptionCodes.ExceedingTotalLimit.ToString());
                else
                    payment.SucceededPayment();

                await _uow.PaymentTransactions.Add(payment);

                await _uow.SaveChangesAsync();

                return BaseResult.Success();
            }
        }
    }
}
