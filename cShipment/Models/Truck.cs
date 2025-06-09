using System.ComponentModel.DataAnnotations;

namespace cShipment.Models
{
    public class Truck
    {
        [Key]
        public int TruckId { get; set; }

        [Required, MaxLength(100)]
        public string Model { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public double Mileage { get; set; }

        public DateTime LastMaintenanceDate { get; set; }

        public int? AssignedDriverId { get; set; }
        public Driver? AssignedDriver { get; set; }

        public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}
