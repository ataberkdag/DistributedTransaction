using Core.Domain.Base;

namespace Stock.Domain.Events
{
    public class StockFailed : IDomainEvent
    {
        public Guid CorrelationId { get; set; }
        public Guid UserId { get; set; }

        public StockFailed(Guid correlationId, Guid userId)
        {
            CorrelationId = correlationId;
            UserId = userId;
        }
    }
}
