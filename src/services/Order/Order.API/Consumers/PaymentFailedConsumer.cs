using AutoMapper;
using MassTransit;
using MediatR;
using Messages.IntegrationEvents;
using Order.Application.Features.Commands;

namespace Order.API.Consumers
{
    public class PaymentFailedConsumer : IConsumer<PaymentFailedIE>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PaymentFailedConsumer(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<PaymentFailedIE> context)
        {
            await _mediator.Send(_mapper.Map<PaymentFailed.Command>(context.Message));
        }
    }
}
