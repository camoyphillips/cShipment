using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cShipment.Models
{
    public class Shipment
    {
        [Key]
        public int ShipmentId { get; set; }

        [Required]
        public string Origin { get; set; } = string.Empty;

        [Required]
        public string Destination { get; set; } = string.Empty;

        [Required]
        public double Distance { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty;

        [ForeignKey("Truck")]
        public int TruckId { get; set; }
        public Truck Truck { get; set; } = null!;

        public ICollection<DriverShipment> DriverShipments { get; set; } = new List<DriverShipment>();
    }
}
