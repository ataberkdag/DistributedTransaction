namespace Messages.IntegrationEvents
{
    public class OrderPlacedIE : IIntegrationEvent
    {
        public Guid CorrelationId { get; set; }
        public Guid UserId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public string Type { get; private set; }

        public OrderPlacedIE(Guid correlationId, Guid userId, List<OrderItemDto> orderItems)
        {
            CorrelationId = correlationId;
            UserId = userId;
            OrderItems = orderItems;
            Type = typeof(OrderPlacedIE).AssemblyQualifiedName;
        }
    }

    public class OrderItemDto
    {
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }

        public OrderItemDto(Guid itemId, int quantity)
        {
            ItemId = itemId;
            Quantity = quantity;
        }
    }
}
