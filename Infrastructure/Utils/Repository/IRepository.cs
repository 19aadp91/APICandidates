using Infrastructure.Utils.Models;
using System.Linq.Expressions;

namespace Infrastructure.Utils.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeProperties = "");
        Task InsertAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task DeleteAsync(object id);
        void DeleteRange(IEnumerable<T> entityToDelete);
        void Update(T entityToUpdate);
        Task<PagedResult<T>> GetPagedAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeProperties = "", int pageIndex = 1, int pageSize = 10);
    }
}
