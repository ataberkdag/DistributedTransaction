using AutoMapper;
using MassTransit;
using MediatR;
using Messages.IntegrationEvents;
using Stock.Application.Features.Commands;

namespace Stock.API.Consumers
{
    public class OrderPlacedConsumer : IConsumer<OrderPlacedIE>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OrderPlacedConsumer(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<OrderPlacedIE> context)
        {
            await _mediator.Send(_mapper.Map<DecreaseStock.Command>(context.Message));
        }
    }
}
