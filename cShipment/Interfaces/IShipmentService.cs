using cShipment.Models;
using cShipment.Models.Dtos; 

namespace cShipment.Interfaces
{
    public interface IShipmentService
    {
        Task<IEnumerable<ShipmentDto>> ListShipments();

        Task<ShipmentDto?> FindShipment(int id);

        Task<ServiceResponse> AddShipment(ShipmentDto shipmentDto);

        Task<ServiceResponse> UpdateShipment(ShipmentDto shipmentDto);

        Task<ServiceResponse> DeleteShipment(int id);

        Task<IEnumerable<ShipmentDto>> ListShipmentsForDriver(int driverId);

        Task<IEnumerable<ShipmentDto>> ListShipmentsForTruck(int truckId);
    }
}