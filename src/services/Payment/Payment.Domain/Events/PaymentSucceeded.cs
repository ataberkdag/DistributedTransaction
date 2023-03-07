using Core.Domain.Base;

namespace Payment.Domain.Events
{
    public class PaymentSucceeded : IDomainEvent
    {
        public Guid CorrelationId { get; set; }
        public Guid UserId { get; set; }

        public PaymentSucceeded(Guid correlationId, Guid userId)
        {
            CorrelationId = correlationId;
            UserId = userId;
        }
    }
}
