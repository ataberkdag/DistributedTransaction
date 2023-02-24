using Core.Domain.Base;
using Order.Domain.Dtos;

namespace Order.Domain.Events
{
    public class OrderPlaced : IDomainEvent
    {
        public Guid CorrelationId { get; private set; }
        public Guid UserId { get; private set; }
        public List<OrderItemDto> OrderItems { get; private set; }

        public OrderPlaced(Guid correlationId, Guid userId, List<OrderItemDto> orderItems)
        {
            CorrelationId = correlationId;
            UserId = userId;
            OrderItems = orderItems;
        }
    }
}
