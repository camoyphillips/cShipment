using System;
using System.ComponentModel.DataAnnotations;

namespace cShipment.Models.Dtos
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for Driver-Shipment assignments.
    /// Used for retrieving and creating/updating assignments via API.
    /// </summary>
    public class DriverShipmentDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the driver-shipment assignment.
        /// Nullable for new assignments (ID will be generated), required for updates/retrievals.
        /// </summary>
        public int? DriverShipmentId { get; set; } // Now includes the ID

        /// <summary>
        /// Gets or sets the ID of the assigned driver.
        /// </summary>
        [Required(ErrorMessage = "Driver ID is required.")]
        public int DriverId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the assigned shipment.
        /// </summary>
        [Required(ErrorMessage = "Shipment ID is required.")]
        public int ShipmentId { get; set; }

        /// <summary>
        /// Gets or sets the role of the driver in this specific shipment assignment.
        /// </summary>
        [StringLength(50, ErrorMessage = "Role cannot exceed 50 characters.")]
        public string? Role { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the driver was assigned to the shipment.
        /// </summary>
        public DateTime AssignedOn { get; set; }

        // --- Properties for displaying related data (read-only) ---

        /// <summary>
        /// Gets or sets the name of the assigned driver.
        /// </summary>
        public string? DriverName { get; set; }

        /// <summary>
        /// Gets or sets the origin location of the assigned shipment.
        /// </summary>
        public string? ShipmentOrigin { get; set; }

        /// <summary>
        /// Gets or sets the destination location of the assigned shipment.
        /// </summary>
        public string? ShipmentDestination { get; set; }
        public string ShipmentStatus { get; internal set; }
    }
}