using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DataUtils
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> GetAll();
        IQueryable<T> Get(Expression<Func<T, bool>> predicate);
        IQueryable<T> Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> Get(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        IQueryable<T> GetMany(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int Count = 0, string includeProperties = "");
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);

    }
}
