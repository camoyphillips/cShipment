using cShipment.Models.Dtos;
using System.Collections.Generic; 

namespace cShipment.Models.ViewModels
{
    public class TruckDetails
    {
        public TruckDto Truck { get; set; } = null!;
        public DriverDto? AssignedDriver { get; set; }
        public List<ShipmentDto> ActiveShipments { get; set; } = new();
    }
}