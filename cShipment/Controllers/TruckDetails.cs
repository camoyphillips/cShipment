using cShipment.Models;
using cShipment.Models.Dtos;

namespace cShipment.Controllers
{
    internal class TruckDetails
    {
        public TruckDto Truck { get; set; }
        public object AssignedDriver { get; set; }
        public List<Shipment> ActiveShipments { get; set; }
    }
}