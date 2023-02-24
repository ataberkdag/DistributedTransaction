using Core.Application.Common;
using MediatR;
using Order.Domain.Dtos;
using Order.Domain.Interfaces;

namespace Order.Application.Features.Commands
{
    public static class PlaceOrder
    {
        public class Command : IRequest<BaseResult<Response>>
        { 
            public Guid UserId { get; set; }
            public List<OrderItemDto> OrderItems { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, BaseResult<Response>>
        {
            private readonly IOrderUnitOfWork _uow;

            public CommandHandler(IOrderUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<BaseResult<Response>> Handle(Command request, CancellationToken cancellationToken)
            {
                // TODO: User check

                var order = Order.Domain.Entities.Order.CreateOrder(request.UserId, request.OrderItems);

                await _uow.Orders.Add(order);

                await _uow.SaveChangesAsync();

                return BaseResult<Response>.Success(new Response { CorrelationId = order.CorrelationId });
            }
        }

        public class Response
        {
            public Guid CorrelationId { get; set; }
        }
    }
}
