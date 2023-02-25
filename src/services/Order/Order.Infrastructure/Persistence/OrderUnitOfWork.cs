using Core.Infrastructure.Persistence;
using Order.Domain.Interfaces;

namespace Order.Infrastructure.Persistence
{
    public class OrderUnitOfWork : BaseUnitOfWork, IOrderUnitOfWork
    {
        private readonly IOrderRepository _orderRepository;
        public OrderUnitOfWork(OrderDbContext context, IServiceProvider provider) : base(context, provider)
        {
            _orderRepository = (IOrderRepository)provider.GetService(typeof(IOrderRepository));
        }

        public IOrderRepository Orders => _orderRepository;
    }
}
