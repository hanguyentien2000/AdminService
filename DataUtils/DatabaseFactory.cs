using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataUtils
{
    public class DatabaseFactory<TContext> : IDatabaseFactory where TContext : DbContext
    {
        private readonly Func<TContext> _contextFactory;
        private TContext _dbContext;

        public DatabaseFactory(Func<TContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public DbContext GetDbContext()
        {
            return _dbContext ??= _contextFactory();
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}
