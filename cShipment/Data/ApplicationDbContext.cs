using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using cShipment.Models;

namespace cShipment.Data
{
    /// <summary>
    /// EF Core database context for cShipment logistics operations.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Core business entities
        public DbSet<Truck> Trucks { get; set; } = null!;
        public DbSet<Driver> Drivers { get; set; } = null!;
        public DbSet<Shipment> Shipments { get; set; } = null!;
        public DbSet<DriverShipment> DriverShipments { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Automatically apply configurations (if any exist)
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Many-to-Many: Driver ↔ Shipment
            modelBuilder.Entity<DriverShipment>()
                .HasKey(ds => new { ds.DriverId, ds.ShipmentId });

            modelBuilder.Entity<DriverShipment>()
                .HasOne(ds => ds.Driver)
                .WithMany(d => d.DriverShipments)
                .HasForeignKey(ds => ds.DriverId);

            modelBuilder.Entity<DriverShipment>()
                .HasOne(ds => ds.Shipment)
                .WithMany(s => s.DriverShipments)
                .HasForeignKey(ds => ds.ShipmentId);

            // One-to-One: Truck → Assigned Driver (nullable)
            modelBuilder.Entity<Truck>()
                .HasOne(t => t.AssignedDriver)
                .WithMany()
                .HasForeignKey(t => t.AssignedDriverId)
                .OnDelete(DeleteBehavior.SetNull);

            // Many-to-One: Shipment → Truck
            modelBuilder.Entity<Shipment>()
                .HasOne(s => s.Truck)
                .WithMany(t => t.Shipments)
                .HasForeignKey(s => s.TruckId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
