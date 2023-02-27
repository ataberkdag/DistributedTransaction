namespace Stock.Domain.Dtos
{
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
