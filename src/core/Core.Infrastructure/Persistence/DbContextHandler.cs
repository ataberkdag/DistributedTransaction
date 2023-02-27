using Core.Application.Services;
using Core.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Persistence
{
    public class DbContextHandler : IDbContextHandler
    {
        private readonly DbContext _context;

        public DbContextHandler(DbContext context)
        {
            _context = context;
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public DbSet<T> Get<T>() where T : BaseRootEntity
        {
            return _context.Set<T>();
        }
    }
}
