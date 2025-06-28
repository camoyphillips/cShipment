using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace cShipment.Models
{
    /// <summary>
    /// Represents a Driver entity in the system, participating in a many-to-many relationship with Shipments.
    /// </summary>
    public class Driver
    {
        /// <summary>
        /// Gets or sets the unique identifier for the driver.
        /// </summary>
        [Key]
        public int DriverId { get; set; }

        /// <summary>
        /// Gets or sets the full name of the driver. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Driver name is required.")]
        [StringLength(100, ErrorMessage = "Driver name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the driver's license number. This field is required.
        /// </summary>
        [Required(ErrorMessage = "License number is required.")]
        [StringLength(50, ErrorMessage = "License number cannot exceed 50 characters.")]
        public string LicenseNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the contact phone number of the driver. Optional.
        /// </summary>
        [StringLength(20, ErrorMessage = "Contact number cannot exceed 20 characters.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? ContactNumber { get; set; } // Added ContactNumber

        // Navigation property for many-to-many with Shipment via DriverShipment
        /// <summary>
        /// Gets or sets the collection of DriverShipment join entities associated with this driver.
        /// </summary>
        public ICollection<DriverShipment> DriverShipments { get; set; } = new List<DriverShipment>();
    }
}