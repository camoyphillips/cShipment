namespace cShipment.Models.Dtos
{
    public class DriverDto
    {
        public int? DriverId { get; set; }

        public required string Name { get; set; }

        public required string LicenseNumber { get; set; }
    }
}
