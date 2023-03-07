using AutoMapper;
using MassTransit;
using MediatR;
using Messages.IntegrationEvents;
using Order.Application.Features.Commands;

namespace Order.API.Consumers
{
    public class PaymentSucceededConsumer : IConsumer<PaymentSucceededIE>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PaymentSucceededConsumer(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<PaymentSucceededIE> context)
        {
            await _mediator.Send(_mapper.Map<PaymentSucceeded.Command>(context.Message));
        }
    }
}
