using cShipment.Models;
using cShipment.Models.Dtos;

namespace cShipment.Interfaces
{
    public interface ITruckService
    {
        Task<IEnumerable<TruckDto>> ListTrucks();
        Task<TruckDto?> FindTruck(int id);
        Task<ServiceResponse> AddTruck(TruckDto truckDto);
        Task<ServiceResponse> UpdateTruck(TruckDto truckDto);
        Task<ServiceResponse> DeleteTruck(int id);

        // Related methods
        Task<IEnumerable<ShipmentDto>> ListShipmentsForTruck(int truckId);
    }
}
