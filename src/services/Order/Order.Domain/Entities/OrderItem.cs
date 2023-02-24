using Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public Guid ItemId { get; private set; }
        public int Quantity { get; private set; }

        public long OrderId { get; private set; }
        public Order Order { get; private set; }

        protected OrderItem()
        {

        }

        private OrderItem(Guid itemId, int quantity)
        {
            ItemId = itemId;
            Quantity = quantity;
        }

        public static OrderItem CreateOrderItem(Guid itemId, int quantity)
        {
            return new OrderItem(itemId, quantity);
        }
    }


}
