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
    ///   kết hợp Database Factory với Unit of Work để: Tạo DbContext một cách chủ động, có kiểm soát  Tái sử dụng context nếu đã tồn tại(giống scoped DI behavior
    ///
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private readonly IDatabaseFactory _databaseFactory;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
            _context = _databaseFactory.GetDbContext();
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            if (_repositories.TryGetValue(typeof(T), out var repo))
            {
                return (IRepository<T>)repo!;
            }

            var repository = new BaseRepository<T>(_databaseFactory);
            _repositories[typeof(T)] = repository;
            return repository;
        }

        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
