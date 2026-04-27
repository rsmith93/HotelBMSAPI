using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBMSData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace HotelBMSData.Context
{
    public class LogsContext: DbContext
    {
        public LogsContext(DbContextOptions<LogsContext> options)
       : base(options)
        {
        }

        public DbSet<LogEntry> Logs => Set<LogEntry>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogEntry>().HasNoKey();
        }
    }
}
