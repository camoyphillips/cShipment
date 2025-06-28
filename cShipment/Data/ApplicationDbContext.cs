using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using cShipment.Models;

namespace cShipment.Data
{
    /// <summary>
    /// EF Core database context for cShipment logistics operations.
    /// Handles entity relationships and schema configurations.
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

        /// <summary>
        /// Configures the model and relationships between entities.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Automatically apply any IEntityTypeConfiguration<T> from this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // -------------------------
            // DriverShipment (many-to-many with extra fields)
            // -------------------------
            modelBuilder.Entity<DriverShipment>()
                .HasKey(ds => ds.DriverShipmentId); // Primary key

            modelBuilder.Entity<DriverShipment>()
                .HasIndex(ds => new { ds.DriverId, ds.ShipmentId })
                .IsUnique(); // Ensure logical uniqueness

            modelBuilder.Entity<DriverShipment>()
                .HasOne(ds => ds.Driver)
                .WithMany(d => d.DriverShipments)
                .HasForeignKey(ds => ds.DriverId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<DriverShipment>()
                .HasOne(ds => ds.Shipment)
                .WithMany(s => s.DriverShipments)
                .HasForeignKey(ds => ds.ShipmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------------------
            // Truck → Assigned Driver (optional one-to-many)
            // -------------------------
            modelBuilder.Entity<Truck>()
                .HasOne(t => t.AssignedDriver)
                .WithMany()
                .HasForeignKey(t => t.AssignedDriverId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // -------------------------
            // Shipment → Truck (required many-to-one)
            // -------------------------
            modelBuilder.Entity<Shipment>()
                .HasOne(s => s.Truck)
                .WithMany(t => t.Shipments)
                .HasForeignKey(s => s.TruckId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
