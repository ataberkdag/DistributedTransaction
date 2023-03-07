namespace Messages.IntegrationEvents
{
    public class PaymentFailedIE : IIntegrationEvent
    {
        public Guid CorrelationId { get; set; }
        public Guid UserId { get; set; }

        public string Type { get; private set; }

        public PaymentFailedIE(Guid correlationId, Guid userId)
        {
            CorrelationId = correlationId;
            UserId = userId;
            Type = typeof(PaymentFailedIE).AssemblyQualifiedName;
        }
    }
}
