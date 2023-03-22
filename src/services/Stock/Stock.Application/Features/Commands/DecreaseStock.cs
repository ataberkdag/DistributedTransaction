using Core.Application.Common;
using Core.Application.Services;
using FluentValidation;
using MediatR;
using Stock.Domain.Dtos;
using Stock.Domain.Entities;
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
                var firstOrderItem = request.OrderItems?.FirstOrDefault();
                if (firstOrderItem == null)
                    throw new ValidationException("OrderItem not found");

                var stock = Stock.Domain.Entities.Stock.Create(firstOrderItem.ItemId, firstOrderItem.Quantity, 12);

                await _uow.Stocks.Add(stock);

                var decreaseResult = stock?
                    .DecreaseStock(request.CorrelationId, request.UserId, firstOrderItem.Quantity, true);

                await this._uow.SaveChangesAsync();

                return BaseResult.Success();
            }
        }
    }
}
