using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace SnackMachine.Domain
{
    public class SnackMachineContext : DbContext
    {
        public SnackMachineContext(DbContextOptions<SnackMachineContext> options) : base(options)
        { }

        public DbSet<SnackMachine> SnackMachines { get; set; }
        public DbSet<Snack> Snacks { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<HeadOffice> HeadOffices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SnackMachine>().Ignore(x => x.MoneyInTransaction);
            modelBuilder.Entity<SnackMachine>().OwnsOne(x => x.MoneyInside).Property(m => m.OneCentCount);
            modelBuilder.Entity<SnackMachine>().OwnsOne(x => x.MoneyInside).Property(m => m.TenCentCount);
            modelBuilder.Entity<SnackMachine>().OwnsOne(x => x.MoneyInside).Property(m => m.QuarterCount);
            modelBuilder.Entity<SnackMachine>().OwnsOne(x => x.MoneyInside).Property(m => m.OneDollarCount);
            modelBuilder.Entity<SnackMachine>().OwnsOne(x => x.MoneyInside).Property(m => m.FiveDollarCount);
            modelBuilder.Entity<SnackMachine>().OwnsOne(x => x.MoneyInside).Property(m => m.TwentyDollarCount);
            
            modelBuilder.Entity<SnackMachine>().HasMany(typeof(Slot), "Slots").WithOne("SnackMachine");

            modelBuilder.Entity<Slot>().OwnsOne(x => x.SnackPile).Property(s => s.Price);
            modelBuilder.Entity<Slot>().OwnsOne(x => x.SnackPile).Property(s => s.Quantity);
            modelBuilder.Entity<Slot>().OwnsOne(x => x.SnackPile).HasOne(s => s.Snack);

            modelBuilder.Entity<SnackMachine>().Ignore(x => x.DomainEvents);
            modelBuilder.Entity<Snack>().Ignore(x => x.DomainEvents);
            modelBuilder.Entity<HeadOffice>().Ignore(x => x.DomainEvents);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            var timestamp = DateTime.Now;
            foreach (var entry in ChangeTracker.Entries()
                .Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                && !e.Metadata.IsOwned()))
            {
                DispatchEvents(entry.Entity as AggregateRoot);
            }
            return base.SaveChanges();
        }

        private void DispatchEvents(AggregateRoot aggregateRoot)
        {
            if(aggregateRoot != null)
            {
                foreach(var domainEvent in aggregateRoot.DomainEvents)
                {
                    DomainEvents.Dispatch(domainEvent);
                }
            }
        }
    }
}