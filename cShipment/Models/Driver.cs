using cShipment.Models;

public class Driver
{
    public int DriverId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;

    public ICollection<DriverShipment> DriverShipments { get; set; } = new List<DriverShipment>(); 
}
