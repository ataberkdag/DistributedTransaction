using Microsoft.EntityFrameworkCore;
using User.BL.Services.Interfaces;
using User.DAL.Persistence;

namespace User.BL.Services.Impl
{
    public class DbContextHandler : IDbContextHandler
    {
        private readonly UserDbContext _context;

        public DbContextHandler(UserDbContext context)
        {
            _context = context;
        }

        public DbSet<T> GetDbSet<T>() where T : class
        {
            return _context.Set<T>();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
