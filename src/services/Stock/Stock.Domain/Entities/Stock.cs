using Core.Domain.Base;
using Stock.Domain.Events;

namespace Stock.Domain.Entities
{
    public class Stock : BaseRootEntity
    {
        public Guid ItemId { get; private set; }
        public int Quantity { get; private set; }

        protected Stock()
        {

        }

        private Stock(Guid itemId, int quantity)
        {
            this.ItemId = itemId;
            this.Quantity = quantity;
        }

        public static Stock Create(Guid itemId, int quantity)
        {
            return new Stock(itemId, quantity);
        }

        public bool DecreaseStock(Guid correlationId, Guid userId, int quantity, bool isLastItem)
        {
            if (this.Quantity < quantity)
            {
                this.AddDomainEvent(new StockFailed(correlationId, userId));
                return false;
            }

            this.Quantity -= quantity;

            if (isLastItem)
                this.AddDomainEvent(new StockDecreased(correlationId, userId));

            return true;
        }
    }
}
