using AutoMapper;
using MassTransit;
using MediatR;
using Messages.IntegrationEvents;
using Report.Application.Features.Commands;

namespace Report.API.Consumers
{
    public class PaymentSucceededEventConsumer : IConsumer<PaymentSucceededIE>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PaymentSucceededEventConsumer(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<PaymentSucceededIE> context)
        {
            await _mediator.Send(_mapper.Map<LogEvent.Command>(context.Message));
        }
    }
}
