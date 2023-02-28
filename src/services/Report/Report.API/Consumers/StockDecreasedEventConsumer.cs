using AutoMapper;
using MassTransit;
using MediatR;
using Messages.IntegrationEvents;
using Report.Application.Features.Commands;

namespace Report.API.Consumers
{
    public class StockDecreasedEventConsumer : IConsumer<StockDecreasedIE>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public StockDecreasedEventConsumer(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<StockDecreasedIE> context)
        {
            await _mediator.Send(_mapper.Map<LogEvent.Command>(context.Message));
        }
    }
}
