using Core.Infrastructure.Persistence;
using Order.Domain.Interfaces;

namespace Order.Infrastructure.Persistence
{
    public class OrderRepository : GenericRepository<Order.Domain.Entities.Order>, IOrderRepository
    {
        public OrderRepository(OrderDbContext context) : base(context)
        {

        }
    }
}
