namespace cShipment.Models.Dtos
{
    public class ShipmentDto
    {
        public int ShipmentId { get; set; }
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public double Distance { get; set; }
        public string Status { get; set; } = string.Empty;
        public int TruckId { get; set; }
    }
}
