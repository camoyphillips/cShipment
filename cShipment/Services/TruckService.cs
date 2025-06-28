using cShipment.Data;
using cShipment.Interfaces;
using cShipment.Models;
using cShipment.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace cShipment.Services
{
    public class TruckService : ITruckService
    {
        private readonly ApplicationDbContext _context;

        public TruckService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TruckDto>> ListTrucks()
        {
            return await _context.Trucks
                .Select(t => new TruckDto 
                {
                    TruckId = t.TruckId,
                    Model = t.Model,
                    Mileage = (int)t.Mileage,
                    LastMaintenanceDate = t.LastMaintenanceDate,
                    AssignedDriverId = t.AssignedDriverId, 
                    AssignedDriverName = t.AssignedDriver != null ? t.AssignedDriver.Name : string.Empty,
                    TruckImagePath = t.TruckImagePath 
                })
                .ToListAsync();
        }

        public async Task<TruckDto?> FindTruck(int id)
        {
            var truck = await _context.Trucks
                .Include(t => t.AssignedDriver)
                .FirstOrDefaultAsync(t => t.TruckId == id);

            return truck == null ? null : new TruckDto
            {
                TruckId = truck.TruckId,
                Model = truck.Model,
                Mileage = (int)truck.Mileage,
                LastMaintenanceDate = truck.LastMaintenanceDate,
                AssignedDriverId = truck.AssignedDriverId, 
                AssignedDriverName = truck.AssignedDriver != null ? truck.AssignedDriver.Name : string.Empty,
                TruckImagePath = truck.TruckImagePath 
            };
        }

        public async Task<ServiceResponse> AddTruck(TruckDto dto)
        {
            var truck = new Truck
            {
                Model = dto.Model ?? string.Empty,
                Mileage = dto.Mileage,
                LastMaintenanceDate = dto.LastMaintenanceDate,
                AssignedDriverId = dto.AssignedDriverId,
                TruckImagePath = dto.TruckImagePath 
            };
            _context.Trucks.Add(truck);
            await _context.SaveChangesAsync();
            return new ServiceResponse { Status = ServiceResponse.ServiceStatus.Created, CreatedId = truck.TruckId };
        }

        public async Task<ServiceResponse> UpdateTruck(TruckDto dto)
        {
            var truck = await _context.Trucks.FindAsync(dto.TruckId);
            if (truck == null)
                return new ServiceResponse { Status = ServiceResponse.ServiceStatus.NotFound };

            truck.Model = dto.Model ?? truck.Model;
            truck.Mileage = dto.Mileage;
            truck.LastMaintenanceDate = dto.LastMaintenanceDate;
            truck.AssignedDriverId = dto.AssignedDriverId;
       
            if (!string.IsNullOrWhiteSpace(dto.TruckImagePath))
            {
                truck.TruckImagePath = dto.TruckImagePath;
            }
       

            await _context.SaveChangesAsync();
            return new ServiceResponse { Status = ServiceResponse.ServiceStatus.Updated };
        }

        public async Task<ServiceResponse> DeleteTruck(int id)
        {
            var truck = await _context.Trucks.FindAsync(id);
            if (truck == null)
                return new ServiceResponse { Status = ServiceResponse.ServiceStatus.NotFound };

            _context.Trucks.Remove(truck);
            await _context.SaveChangesAsync();
            return new ServiceResponse { Status = ServiceResponse.ServiceStatus.Deleted };
        }

        public async Task<IEnumerable<ShipmentDto>> ListShipmentsForTruck(int truckId)
        {
            return await _context.Shipments
                .Where(s => s.TruckId == truckId)
                .Select(s => new ShipmentDto
                {
                    ShipmentId = s.ShipmentId,
                    Origin = s.Origin,
                    Destination = s.Destination,
                    Distance = (int)s.Distance,
                    Status = s.Status,
                    TruckId = s.TruckId
                })
                .ToListAsync();
        }
    }
}