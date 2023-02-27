using Core.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Core.Application.Services
{
    public interface IDbContextHandler
    {
        public DbSet<T> Get<T>() where T : BaseRootEntity;
        public Task Commit();
    }
}
