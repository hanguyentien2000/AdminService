using Microsoft.EntityFrameworkCore;

namespace DataUtils
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private readonly DbContext _dbContext;

        public DatabaseFactory(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbContext GetDbContext() => _dbContext;
    }
}
