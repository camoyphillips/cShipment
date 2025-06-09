using System.ComponentModel.DataAnnotations;

namespace cShipment.Models
{
    /// <summary>
    /// Represents a customer who can place shipment orders.
    /// </summary>
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string CustomerEmail { get; set; } = string.Empty;

    }

    /// <summary>
    /// Data Transfer Object (DTO) for exposing limited customer info via API.
    /// </summary>
    public class CustomerDto
    {
        public int? CustomerId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [EmailAddress]
        [MaxLength(150)]
        public string? CustomerEmail { get; set; }
    }
}