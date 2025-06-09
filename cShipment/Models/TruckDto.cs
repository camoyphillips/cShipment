namespace cShipment.Models.Dtos
{
    public class TruckDto
    {
        public int TruckId { get; set; }
        public string Model { get; set; } = string.Empty;
        public double Mileage { get; set; }
        public DateTime LastMaintenanceDate { get; set; }
        public int? AssignedDriverId { get; set; }
    }
}
