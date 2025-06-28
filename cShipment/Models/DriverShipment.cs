using System;
using System.ComponentModel.DataAnnotations;

namespace cShipment.Models
{
    /// <summary>
    /// Represents the join entity for the many-to-many relationship between Driver and Shipment.
    /// This entity now includes its own primary key and additional properties for the assignment.
    /// </summary>
    public class DriverShipment
    {
        /// <summary>
        /// Gets or sets the unique identifier for the driver-shipment assignment.
        /// This acts as the primary key for the join table, allowing individual referencing.
        /// </summary>
        [Key]
        public int DriverShipmentId { get; set; } 

        /// <summary>
        /// Gets or sets the foreign key for the Driver.
        /// </summary>
        public int DriverId { get; set; }
        /// <summary>
        /// Gets or sets the navigation property to the Driver.
        /// </summary>
        public Driver Driver { get; set; } = null!;

        /// <summary>
        /// Gets or sets the foreign key for the Shipment.
        /// </summary>
        public int ShipmentId { get; set; }
        /// <summary>
        /// Gets or sets the navigation property to the Shipment.
        /// </summary>
        public Shipment Shipment { get; set; } = null!;

        /// <summary>
        /// Gets or sets the role of the driver in this specific shipment assignment (e.g., "Primary", "Assistant").
        /// </summary>
        [StringLength(50, ErrorMessage = "Role cannot exceed 50 characters.")]
        public string? Role { get; set; } // Added Role property

        /// <summary>
        /// Gets or sets the date and time when the driver was assigned to the shipment.
        /// </summary>
        public DateTime AssignedOn { get; set; } = DateTime.UtcNow; 
    }
}