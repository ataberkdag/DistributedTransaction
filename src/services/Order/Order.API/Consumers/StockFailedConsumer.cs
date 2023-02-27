using MassTransit;
using Messages.IntegrationEvents;

namespace Order.API.Consumers
{
    public class StockFailedConsumer : IConsumer<StockFailedIE>
    {
        public Task Consume(ConsumeContext<StockFailedIE> context)
        {
            Console.WriteLine("Stock Failed event consumed!");

            return Task.CompletedTask;
        }
    }
}
