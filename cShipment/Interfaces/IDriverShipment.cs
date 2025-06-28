using cShipment.Models;
using cShipment.Models.Dtos; 

namespace cShipment.Interfaces
{
    public interface IDriverShipmentService
    {
        Task<ServiceResponse> AssignDriverToShipment(int driverId, int shipmentId, string? role = null);

        Task<ServiceResponse> UnassignDriverFromShipment(int driverId, int shipmentId);

        Task<IEnumerable<DriverDto>> ListDriversForShipment(int shipmentId);

        Task<IEnumerable<ShipmentDto>> ListShipmentsForDriver(int driverId);
        Task ListDriverShipmentsForShipment(int id);
    }
}