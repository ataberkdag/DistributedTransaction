namespace Messages.IntegrationEvents
{
    public class PaymentSucceededIE : IIntegrationEvent
    {
        public Guid CorrelationId { get; private set; }
        public Guid UserId { get; private set; }

        public string Type { get; private set; }

        public PaymentSucceededIE(Guid correlationId, Guid userId)
        {
            CorrelationId = correlationId;
            UserId = userId;
            Type = typeof(PaymentSucceededIE).AssemblyQualifiedName;
        }
    }
}
