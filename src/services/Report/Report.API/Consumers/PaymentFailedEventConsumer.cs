using AutoMapper;
using MassTransit;
using MediatR;
using Messages.IntegrationEvents;
using Report.Application.Features.Commands;

namespace Report.API.Consumers
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedIE>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PaymentFailedEventConsumer(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<PaymentFailedIE> context)
        {
            await _mediator.Send(_mapper.Map<LogEvent.Command>(context.Message));
        }
    }
}
