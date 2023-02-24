using Core.Domain.Base;
using System.Linq.Expressions;

namespace Core.Domain.Interfaces
{
    public interface IGenericRepository<T> where T: BaseEntity
    {
        Task Add(T item);
        void Delete(T item);
        Task<T> GetById(long id);
        Task<List<T>> FindByQuery(Expression<Func<T, bool>> expression, bool tracking = true);
    }
}
