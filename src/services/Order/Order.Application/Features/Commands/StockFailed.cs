using Core.Application.Common;
using Core.Application.Exceptions;
using MediatR;
using Order.Domain.Interfaces;

namespace Order.Application.Features.Commands
{
    public class StockFailed : IRequest<BaseResult>
    {
        public Guid CorrelationId { get; private set; }
        public Guid UserId { get; private set; }
    }

    public class StockFailedCommandHandler : IRequestHandler<StockFailed, BaseResult>
    {
        private readonly IOrderUnitOfWork _uow;
        public StockFailedCommandHandler(IOrderUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<BaseResult> Handle(StockFailed request, CancellationToken cancellationToken)
        {
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
