using Core.Domain.Base;

namespace Payment.Domain.Events
{
    public class PaymentFailed : IDomainEvent
    {
        public Guid CorrelationId { get; set; }
        public Guid UserId { get; set; }

        public PaymentFailed(Guid correlationId, Guid userId)
        {
            CorrelationId = correlationId;
            UserId = userId;
        }
    }
}
