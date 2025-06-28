using cShipment.Models.Dtos;

namespace cShipment.Models.ViewModels
{
    /// <summary>
    /// Combines driver data with related truck and assigned shipments for view rendering.
    /// </summary>
    public class DriverDetails
    {
        public DriverDto Driver { get; set; } = null!;
        public string? AssignedTruckModel { get; set; }
        public List<ShipmentDto> AssignedShipments { get; set; } = new();
    }
}
