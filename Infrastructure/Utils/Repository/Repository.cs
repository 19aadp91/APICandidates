using Infrastructure.Utils.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Utils.Repository
{
    public class Repository<T>(DbContext context) : IRepository<T> where T : class
    {
        private readonly DbContext _context = context;
        private readonly DbSet<T> _entities = context.Set<T>();

        public virtual async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeProperties = "")
        {
            return await BuildQuery(filter, orderBy, includeProperties).AsNoTracking().ToListAsync();
        }

        public virtual async Task InsertAsync(T entity) => await _entities.AddAsync(entity);

        public virtual async Task AddRangeAsync(IEnumerable<T> entities) => await _entities.AddRangeAsync(entities);

        public virtual async Task DeleteAsync(object id)
        {
            T? entityToDelete = await _entities.FindAsync(id);
            Delete(entityToDelete!);
        }

        public virtual void DeleteRange(IEnumerable<T> entityToDelete)
        {
            if (entityToDelete != null && entityToDelete.Any())
            {
                if (_context.Entry(entityToDelete.First()).State == EntityState.Detached)
                {
                    _entities.AttachRange(entityToDelete);
                }
                _entities.RemoveRange(entityToDelete);
            }
        }

        public virtual void Update(T entityToUpdate)
        {
            _entities.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual async Task<PagedResult<T>> GetPagedAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "",
            int pageIndex = 1,
            int pageSize = 10)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var totalItems = await query.CountAsync();

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<T>
            {
                TotalItems = totalItems,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Items = items
            };
        }

        private void Delete(T entityToDelete)
        {
            if (entityToDelete != null)
            {
                if (_context.Entry(entityToDelete).State == EntityState.Detached)
                {
                    _entities.Attach(entityToDelete);
                }
                _entities.Remove(entityToDelete);
            }
        }

        private IQueryable<T> BuildQuery(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = _entities;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query;
        }
    }
}
