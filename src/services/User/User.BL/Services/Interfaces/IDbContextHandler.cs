using Microsoft.EntityFrameworkCore;

namespace User.BL.Services.Interfaces
{
    public interface IDbContextHandler
    {
        public DbSet<T> GetDbSet<T>() where T : class;
        public Task SaveChangesAsync();
    }
}
