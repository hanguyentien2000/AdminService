﻿using System;
using AdminService.Insfrastructure.Databases;
using DataUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Debug;

namespace AdminService.Insfrastructure
{
    public class DataContext : AppDbContext
    {
        public static readonly ILoggerFactory loggerFactory = new LoggerFactory(new[] {
              new DebugLoggerProvider()
        });

        public DataContext(DbContextOptions<AppDbContext> options)
       : base(options)
        {
        }
        public DbSet<IdmUsers> IdmUsers { get; set; }  // Ví dụ entity

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfiguration(new OrderConfiguration());
        }
    }
}
