namespace Messages.IntegrationEvents
{
    public class StockFailedIE : IIntegrationEvent
    {
        public Guid CorrelationId { get; private set; }
        public Guid UserId { get; private set; }
        public string Type { get; private set; }

        public StockFailedIE(Guid correlationId, Guid userId)
        {
            CorrelationId = correlationId;
            UserId = userId;
        }
    }
}
