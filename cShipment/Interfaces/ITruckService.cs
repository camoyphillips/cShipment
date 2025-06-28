using cShipment.Models;
using cShipment.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cShipment.Interfaces
{
    public interface ITruckService
    {
        Task<IEnumerable<TruckDto>> ListTrucks();
        Task<TruckDto?> FindTruck(int id); 
        Task<ServiceResponse> AddTruck(TruckDto dto);
        Task<ServiceResponse> UpdateTruck(TruckDto dto);
        Task<ServiceResponse> DeleteTruck(int id);
        Task<IEnumerable<ShipmentDto>> ListShipmentsForTruck(int truckId);

     
    }
}