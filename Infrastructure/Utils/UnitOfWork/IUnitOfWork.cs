using Infrastructure.Utils.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Utils.UnitOfWork
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        IRepository<TItem> Repository<TItem>() where TItem : class;
        void Save();
        Task SaveAsync();
    }
}
