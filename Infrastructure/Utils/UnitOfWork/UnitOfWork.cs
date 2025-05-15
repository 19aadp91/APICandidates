using Infrastructure.Utils.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Utils.UnitOfWork
{
    public class UnitOfWork<T>(T context) : IUnitOfWork<T> where T : DbContext
    {
        private readonly T _baseContext = context;
        private readonly Dictionary<string, object?> _repositories = [];

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IRepository<TItem> Repository<TItem>() where TItem : class
        {
            string name = typeof(TItem).Name;
            if (!_repositories.ContainsKey(name))
            {
                object? repository = Activator.CreateInstance(typeof(Repository<>).MakeGenericType(new Type[] { typeof(TItem) }), new object[] { _baseContext });
                _repositories.Add(name, repository);
            }
            return (_repositories[name] as Repository<TItem>)!;
        }

        public void Save()
        {
            _baseContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _baseContext.SaveChangesAsync();
        }


        #region private methods
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _baseContext.Dispose();
                }
            }
            disposed = true;
        }
        #endregion
    }
}
