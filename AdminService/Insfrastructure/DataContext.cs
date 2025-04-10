using System;
using AdminService.Insfrastructure.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Debug;

namespace AdminService.Insfrastructure
{
    public class DataContext : DbContext
    {
        public static readonly ILoggerFactory loggerFactory = new LoggerFactory(new[] {
              new DebugLoggerProvider()
        });

        public DataContext(DbContextOptions<DataContext> options)
       : base(options)
        {
        }
        public DbSet<IdmUsers> IdmUsers { get; set; }  // Ví dụ entity

    }
}
