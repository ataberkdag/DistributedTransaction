using Core.Application.Common;
using FluentValidation;
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

        #region Validators

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("User should be specified!");

                RuleFor(x => x.OrderItems).NotNull().WithMessage("Order items is required!");

                RuleForEach(x => x.OrderItems).SetValidator(new OrderItemDtoValidator());
            }
        }

        public class OrderItemDtoValidator : AbstractValidator<OrderItemDto>
        {
            public OrderItemDtoValidator()
            {
                RuleFor(x => x.ItemId).NotEmpty().WithMessage("Item should be specified!");

                RuleFor(x => x.Quantity).NotEqual(0).WithMessage("Quantity can not be zero!");

                RuleFor(x => x.Quantity).LessThanOrEqualTo(100).WithMessage("Quantity can not be over hundred!");
            }
        }

        #endregion Validators

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
