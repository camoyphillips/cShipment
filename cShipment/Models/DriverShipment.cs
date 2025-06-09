namespace cShipment.Models
{
    public class DriverShipment
    {
        public int DriverId { get; set; }
        public Driver Driver { get; set; } = null!;

        public int ShipmentId { get; set; }
        public Shipment Shipment { get; set; } = null!;

        public DateTime AssignedOn { get; set; } 
        public string? Role { get; set; }   
    }
}
