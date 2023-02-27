namespace Messages.IntegrationEvents
{
    public class StockDecreasedIE : IIntegrationEvent
    {
        public string Type { get; private set; }
        public Guid UserId { get; private set; }
        public Guid CorrelationId { get; private set; }
        public decimal TotalAmount { get; private set; }

        public StockDecreasedIE(Guid correlationId, Guid userId, decimal totalAmount)
        {
            Type = typeof(StockDecreasedIE).AssemblyQualifiedName;
            CorrelationId = correlationId;
            UserId = userId;
            TotalAmount = totalAmount;
        }
    }
}
