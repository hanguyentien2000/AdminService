using DataUtils;
using Microsoft.EntityFrameworkCore;

namespace AdminService.Insfrastructure
{
    public class AdminServiceDatabaseFactory : IDatabaseFactory
    {
        private readonly AdminDataContext _context;

        public AdminServiceDatabaseFactory(AdminDataContext context)
        {
            _context = context;
        }

        public AdminDataContext GetDbContext() => _context;

        DbContext IDatabaseFactory.GetDbContext()
        {
            return GetDbContext();
        }
    }
}
