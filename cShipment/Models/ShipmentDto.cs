namespace cShipment.Models.Dtos
{
    public class ShipmentDto
    {
        public int ShipmentId { get; set; }
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public int Distance { get; set; }
        public string Status { get; set; } = string.Empty;

        // Foreign key to Truck
        public int TruckId { get; set; } // Assumed non-nullable based on your Shipment entity

        // New property to include the associated Truck's Model for display
        public string? TruckModel { get; set; }
    }
}