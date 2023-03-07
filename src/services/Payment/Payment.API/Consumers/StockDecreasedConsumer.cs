using MassTransit;
using Messages.IntegrationEvents;

namespace Payment.API.Consumers
{
    public class StockDecreasedConsumer : IConsumer<StockDecreasedIE>
    {
        public Task Consume(ConsumeContext<StockDecreasedIE> context)
        {
            throw new NotImplementedException();
        }
    }
}
