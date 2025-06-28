namespace cShipment.Models.Dtos
{
    public class TruckDto
    {
        public int TruckId { get; set; }
        public string Model { get; set; } = string.Empty;
        public int Mileage { get; set; }
        public DateTime LastMaintenanceDate { get; set; }

        public int? AssignedDriverId { get; set; }
        public string? AssignedDriverName { get; set; }

        public string? TruckImagePath { get; set; }
        public bool HasTruckImage => !string.IsNullOrWhiteSpace(TruckImagePath);
    }
}
