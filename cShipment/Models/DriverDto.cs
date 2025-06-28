using System.ComponentModel.DataAnnotations;

namespace cShipment.Models.Dtos
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for Driver entities.
    /// Used to transfer driver data between the service layer and the presentation layer,
    /// including validation attributes for input forms.
    /// </summary>
    public class DriverDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the driver. Nullable for new drivers, non-nullable for existing.
        /// </summary>
        public int DriverId { get; set; } // Changed to int

        /// <summary>
        /// Gets or sets the full name of the driver. Required.
        /// </summary>
        [Required(ErrorMessage = "Driver name is required.")]
        [StringLength(100, ErrorMessage = "Driver name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty; 

        /// <summary>
        /// Gets or sets the driver's license number. Required.
        /// </summary>
        [Required(ErrorMessage = "License number is required.")]
        [StringLength(50, ErrorMessage = "License number cannot exceed 50 characters.")]
        public string LicenseNumber { get; set; } = string.Empty; 

        /// <summary>
        /// Gets or sets the contact phone number of the driver. Optional.
        /// </summary>
        [StringLength(20, ErrorMessage = "Contact number cannot exceed 20 characters.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? ContactNumber { get; set; } 
    }
}