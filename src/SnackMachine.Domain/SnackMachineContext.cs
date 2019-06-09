using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnackMachine.Domain
{
    public class SnackMachineContext : DbContext
    {
        public SnackMachineContext(DbContextOptions<SnackMachineContext> options) : base(options)
        { }

        public DbSet<SnackMachine> SnackMachines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SnackMachine>().Ignore(x => x.MoneyInTransaction);
            modelBuilder.Entity<SnackMachine>().OwnsOne(x => x.MoneyInside).Property(m => m.OneCentCount);
            modelBuilder.Entity<SnackMachine>().OwnsOne(x => x.MoneyInside).Property(m => m.TenCentCount);
            modelBuilder.Entity<SnackMachine>().OwnsOne(x => x.MoneyInside).Property(m => m.QuarterCount);
            modelBuilder.Entity<SnackMachine>().OwnsOne(x => x.MoneyInside).Property(m => m.OneDollarCount);
            modelBuilder.Entity<SnackMachine>().OwnsOne(x => x.MoneyInside).Property(m => m.FiveDollarCount);
            modelBuilder.Entity<SnackMachine>().OwnsOne(x => x.MoneyInside).Property(m => m.TwentyDollarCount);
        }
    }
}