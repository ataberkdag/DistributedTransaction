using Core.Application.Common;
using Core.Application.Exceptions;
using Core.Application.Services;
using MediatR;
using Order.Domain.Interfaces;

namespace Order.Application.Features.Commands
{
    public static class StockFailed
    {
        public class Command : IRequest<BaseResult>
        {
            public Guid CorrelationId { get; private set; }
            public Guid UserId { get; private set; }
        }

        public class CommandHandler : IRequestHandler<Command, BaseResult>
        {
            private readonly IOrderUnitOfWork _uow;
            private readonly IDistributedLockManager _dlm;
            public CommandHandler(IOrderUnitOfWork uow,
                IDistributedLockManager dlm)
            {
                _uow = uow;
                _dlm = dlm;
            }

            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                // Trying to unlock User.
                await _dlm.UnLock(request.UserId.ToString());

                var order = (await _uow.Orders.FindByQuery(x => x.CorrelationId == request.CorrelationId)).FirstOrDefault();

                // TODO: Rule Engine
                if (order == null)
                    throw new BusinessException(BusinessExceptionCodes.OrderNotFound.GetHashCode().ToString(), BusinessExceptionCodes.OrderNotFound.ToString());

                if (order.Status != Domain.Entities.OrderStatus.Processing)
                    throw new BusinessException(BusinessExceptionCodes.OrderProcessed.GetHashCode().ToString(), BusinessExceptionCodes.OrderProcessed.ToString());

                order.FailOrder("Stock failed");

                await _uow.SaveChangesAsync();

                return BaseResult.Success();
            }
        }
    }
    
}
