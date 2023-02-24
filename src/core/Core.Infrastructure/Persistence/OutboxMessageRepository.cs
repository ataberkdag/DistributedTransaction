using Core.Domain.Entities;
using Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Persistence
{
    public class OutboxMessageRepository : GenericRepository<OutboxMessage>, IGenericRepository<OutboxMessage>
    {
        public OutboxMessageRepository(DbContext context) : base(context)
        {

        }
    }
}
