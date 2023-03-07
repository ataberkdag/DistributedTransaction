using Core.Application.Common;
using Core.Application.Exceptions;
using MediatR;
using Order.Domain.Interfaces;

namespace Order.Application.Features.Commands
{
    public static class PaymentSucceeded
    {
        public class Command : IRequest<BaseResult>
        {
            public Guid CorrelationId { get; private set; }
            public Guid UserId { get; private set; }
        }

        public class CommandHandler : IRequestHandler<Command, BaseResult>
        {
            private readonly IOrderUnitOfWork _uow;
            public CommandHandler(IOrderUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var order = (await _uow.Orders.FindByQuery(x => x.CorrelationId == request.CorrelationId)).FirstOrDefault();

                // TODO: Rule Engine
                if (order == null)
                    throw new BusinessException(BusinessExceptionCodes.OrderNotFound.GetHashCode().ToString(), BusinessExceptionCodes.OrderNotFound.ToString());

                if (order.Status != Domain.Entities.OrderStatus.Processing)
                    throw new BusinessException(BusinessExceptionCodes.OrderProcessed.GetHashCode().ToString(), BusinessExceptionCodes.OrderProcessed.ToString());

                order.CompleteOrder();

                await _uow.SaveChangesAsync();

                return BaseResult.Success();
            }
        }
    }
}
