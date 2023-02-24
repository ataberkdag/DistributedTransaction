using Core.Domain.Base;
using Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.Infrastructure.Persistence
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly DbContext _context;

        public GenericRepository(DbContext context)
        {
            _context = context;
        }

        public async Task Add(T item)
        {
            await _context.Set<T>().AddAsync(item);
        }

        public void Delete(T item)
        {
            _context.Set<T>().Remove(item);
        }

        public async Task<List<T>> FindByQuery(Expression<Func<T, bool>> expression, bool tracking = true)
        {
            var queryable = (IQueryable<T>)_context.Set<T>();

            if (tracking is false)
                queryable = queryable.AsNoTracking();

            return await queryable.Where(expression).ToListAsync();
        }

        public async Task<T> GetById(long id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
    }
}
