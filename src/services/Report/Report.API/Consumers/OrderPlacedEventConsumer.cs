using AutoMapper;
using MassTransit;
using MediatR;
using Messages.IntegrationEvents;
using Report.Application.Features.Commands;

namespace Report.API.Consumers
{
    public class OrderPlacedEventConsumer : IConsumer<OrderPlacedIE>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OrderPlacedEventConsumer(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<OrderPlacedIE> context)
        {
            await _mediator.Send(_mapper.Map<LogEvent.Command>(context.Message));
        }
    }
}
