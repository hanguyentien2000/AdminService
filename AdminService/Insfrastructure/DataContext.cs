using System;
using AdminService.Insfrastructure.Databases;
using DataUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Debug;

namespace AdminService.Insfrastructure
{
    public class AdminDataContext : DbContext
    {
        public static readonly ILoggerFactory loggerFactory = new LoggerFactory(new[] {
              new DebugLoggerProvider()
        });

        public AdminDataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<IdmUsers> IdmUsers { get; set; }  // Ví dụ entity
        public DbSet<IdmRoles> IdmRoles { get; set; }
        public DbSet<IdmUsersInRoles> IdmUsersInRoles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
