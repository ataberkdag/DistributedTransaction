using Core.Domain.Interfaces;

namespace Order.Domain.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order.Domain.Entities.Order>
    {

    }
}
