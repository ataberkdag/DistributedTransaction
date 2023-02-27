using Core.Application.Common;
using Core.Application.Services;
using MediatR;
using Stock.Domain.Dtos;
using Stock.Domain.Interfaces;

namespace Stock.Application.Features.Commands
{
    public static class DecreaseStock
    {
        public class Command : IRequest<BaseResult>
        {
            public Guid CorrelationId { get; set; }
            public Guid UserId { get; set; }
            public List<OrderItemDto> OrderItems { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, BaseResult>
        {
            private readonly IStockUnitOfWork _uow;
            private readonly ICacheProvider _cache;

            public CommandHandler(IStockUnitOfWork uow, ICacheProvider cache)
            {
                _uow = uow;
                _cache = cache;
            }

            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var lastOrderItem = request.OrderItems?.Last();
                foreach (var orderItem in request.OrderItems)
                {
                    var stock = (await _uow.Stocks.FindByQuery(x => x.ItemId == orderItem.ItemId)).FirstOrDefault();

                    if (stock == null)
                    {
                        stock = Stock.Domain.Entities.Stock.Create(orderItem.ItemId, orderItem.Quantity);

                        await _uow.Stocks.Add(stock);
                    }

                    var decreaseResult = stock?
                        .DecreaseStock(request.CorrelationId, request.UserId, orderItem.Quantity, orderItem.Equals(lastOrderItem));

                    if (decreaseResult == false)
                        break;

                }

                await this._uow.SaveChangesAsync();

                return BaseResult.Success();
            }
        }
    }
}
