using AutoMapper;
using Core.Application.Common;
using Core.Application.Exceptions;
using Core.Application.Services;
using FluentValidation;
using MediatR;
using Order.Application.Models.Contracts;
using Order.Application.Services;
using Order.Domain.Dtos;
using Order.Domain.Interfaces;

namespace Order.Application.Features.Commands
{
    public static class PlaceOrder
    {
        public class Command : IRequest<BaseResult>
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

                RuleFor(x => x.OrderItems).NotEmpty().WithMessage("Order items is required!");

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

        #region Mapper
        public class OrderPlacedProfile : Profile
        {
            public OrderPlacedProfile()
            {
                CreateMap<Command, CheckLimitRequest>();
            }
        }
        #endregion Mapper

        public class CommandHandler : IRequestHandler<Command, BaseResult>
        {
            private readonly IOrderUnitOfWork _uow;
            private readonly ILimitService _limitService;
            private readonly IMapper _mapper;
            private readonly IDistributedLockManager _dlm;

            public CommandHandler(IOrderUnitOfWork uow, 
                ILimitService limitService, 
                IMapper mapper,
                IDistributedLockManager dlm)
            {
                _uow = uow;
                _limitService = limitService;
                _mapper = mapper;
                _dlm = dlm;
            }

            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                // Trying to lock User. Throw exception if it failure
                if (!await _dlm.Lock(request.UserId.ToString()))
                    throw new BusinessException(BusinessExceptionCodes.AnotherOrderProcessing.GetHashCode().ToString(), BusinessExceptionCodes.AnotherOrderProcessing.ToString());

                await _limitService.CheckLimit(_mapper.Map<CheckLimitRequest>(request));

                var order = Order.Domain.Entities.Order.PlaceOrder(request.UserId, request.OrderItems);

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
