using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataUtils
{

    /// <summary>
    ///   kết hợp Database Factory với Unit of Work để:     Tạo DbContext một cách chủ động, có kiểm soát  Tái sử dụng context nếu đã tồn tại(giống scoped DI behavior
    ///
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory _dbFactory;
        private readonly ConcurrentDictionary<Type, object> _repositories = new();

        public UnitOfWork(IDatabaseFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);
            if (_repositories.TryGetValue(type, out var repo))
            {
                return (IRepository<TEntity>)repo;
            }

            var repoInstance = new Repository<TEntity>(_dbFactory.GetDbContext());
            _repositories[type] = repoInstance;
            return repoInstance;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbFactory.GetDbContext().SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbFactory.Dispose();
        }
    }
}
