using AutoMapper;
using MassTransit;
using MediatR;
using Messages.IntegrationEvents;
using Report.Application.Features.Commands;

namespace Report.API.Consumers
{
    public class StockFailedEventConsumer : IConsumer<StockFailedIE>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public StockFailedEventConsumer(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<StockFailedIE> context)
        {
            await _mediator.Send(_mapper.Map<LogEvent.Command>(context.Message));
        }
    }
}
