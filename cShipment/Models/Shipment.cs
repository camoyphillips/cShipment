using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cShipment.Models
{
    /// <summary>
    /// Represents a shipment entry in the logistics system.
    /// </summary>
    public class Shipment
    {
        [Key]
        public int ShipmentId { get; set; }

        [Required]
        [StringLength(100)]
        public string Origin { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Destination { get; set; } = string.Empty;

        [Required]
        public int Distance { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        // Foreign Key to Truck
        [ForeignKey("Truck")]
        public int TruckId { get; set; }

        public Truck Truck { get; set; } = null!;

        // Many-to-many with Driver via DriverShipment
        public ICollection<DriverShipment> DriverShipments { get; set; } = new List<DriverShipment>();
    }
}
