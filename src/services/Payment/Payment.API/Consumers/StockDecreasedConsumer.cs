using AutoMapper;
using MassTransit;
using MediatR;
using Messages.IntegrationEvents;
using Payment.Application.Features.Commands;

namespace Payment.API.Consumers
{
    public class StockDecreasedConsumer : IConsumer<StockDecreasedIE>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public StockDecreasedConsumer(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<StockDecreasedIE> context)
        {
            await _mediator.Send(_mapper.Map<DoPayment.Command>(context.Message));
        }
    }
}
