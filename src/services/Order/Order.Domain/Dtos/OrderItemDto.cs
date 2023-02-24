namespace Order.Domain.Dtos
{
    public class OrderItemDto
    {
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
