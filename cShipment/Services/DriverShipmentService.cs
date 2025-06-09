using cShipment.Data;
using cShipment.Interfaces;
using cShipment.Models;
using cShipment.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace cShipment.Services
{
    public class DriverShipmentService : IDriverShipmentService
    {
        private readonly ApplicationDbContext _context;

        public DriverShipmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse> AssignDriverToShipment(int driverId, int shipmentId, string? role = null)
        {
            bool exists = await _context.DriverShipments
                .AnyAsync(ds => ds.DriverId == driverId && ds.ShipmentId == shipmentId);

            if (exists)
            {
                return new ServiceResponse
                {
                    Status = ServiceResponse.ServiceStatus.Error,
                    Messages = new() { "Driver already assigned to this shipment." }
                };
            }

            var driverShipment = new DriverShipment
            {
                DriverId = driverId,
                ShipmentId = shipmentId,
                AssignedOn = DateTime.UtcNow,
                Role = role
            };

            _context.DriverShipments.Add(driverShipment);
            await _context.SaveChangesAsync();

            return new ServiceResponse { Status = ServiceResponse.ServiceStatus.Created };
        }

        public async Task<IEnumerable<DriverDto>> ListDriversForShipment(int shipmentId)
        {
            return await _context.DriverShipments
                .Where(ds => ds.ShipmentId == shipmentId)
                .Select(ds => new DriverDto
                {
                    DriverId = ds.Driver.DriverId,
                    Name = ds.Driver.Name,
                    LicenseNumber = ds.Driver.LicenseNumber
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ShipmentDto>> ListShipmentsForDriver(int driverId)
        {
            return await _context.DriverShipments
                .Where(ds => ds.DriverId == driverId)
                .Select(ds => new ShipmentDto
                {
                    ShipmentId = ds.Shipment.ShipmentId,
                    Origin = ds.Shipment.Origin,
                    Destination = ds.Shipment.Destination,
                    Distance = ds.Shipment.Distance,
                    Status = ds.Shipment.Status,
                    TruckId = ds.Shipment.TruckId
                })
                .ToListAsync();
        }

        public async Task<ServiceResponse> UnassignDriverFromShipment(int driverId, int shipmentId)
        {
            var driverShipment = await _context.DriverShipments
                .FirstOrDefaultAsync(ds => ds.DriverId == driverId && ds.ShipmentId == shipmentId);

            if (driverShipment == null)
            {
                return new ServiceResponse { Status = ServiceResponse.ServiceStatus.NotFound };
            }

            _context.DriverShipments.Remove(driverShipment);
            await _context.SaveChangesAsync();

            return new ServiceResponse { Status = ServiceResponse.ServiceStatus.Deleted };
        }
    }
}
