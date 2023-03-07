using Core.Domain.Base;
using Order.Domain.Dtos;
using Order.Domain.Events;

namespace Order.Domain.Entities
{
    public class Order : BaseRootEntity
    {
        public Guid CorrelationId { get; private set; }
        public OrderStatus Status { get; private set; }
        public string StatusDescription { get; private set; }
        public Guid UserId { get; private set; }

        public List<OrderItem> OrderItems { get; private set; }

        protected Order()
        {
            OrderItems = new List<OrderItem>();
        }

        private Order(Guid userId, List<OrderItemDto> orderItemDtos)
        {
            CorrelationId = Guid.NewGuid();
            Status = OrderStatus.Processing;
            StatusDescription = OrderStatus.Processing.ToString();
            UserId = userId;

            OrderItems = orderItemDtos.Select(dto => OrderItem.CreateOrderItem(dto.ItemId, dto.Quantity)).ToList();

            this.AddDomainEvent(new OrderPlaced(CorrelationId, UserId, orderItemDtos));
        }

        public void CompleteOrder()
        {
            Status = OrderStatus.Completed;
            StatusDescription = OrderStatus.Completed.ToString();
        }

        public void FailOrder(string description)
        {
            Status = OrderStatus.Failed;
            StatusDescription = description;
        }

        public static Order PlaceOrder(Guid userId, List<OrderItemDto> orderItemDtos)
        {
            return new Order(userId, orderItemDtos);
        }
    }

    public enum OrderStatus
    {
        Processing = 0,
        Completed = 1,
        Failed = 2
    }
}
