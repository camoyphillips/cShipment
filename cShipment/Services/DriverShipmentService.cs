using cShipment.Data;
using cShipment.Interfaces;
using cShipment.Models;
using cShipment.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace cShipment.Services
{
    /// <summary>
    /// Provides concrete implementations for managing driver-shipment assignments.
    /// </summary>
    public class DriverShipmentService : IDriverShipmentService
    {
        private readonly ApplicationDbContext _context;

        public DriverShipmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DriverShipmentDto>> ListAllDriverShipments()
        {
            return await _context.DriverShipments
                .Include(ds => ds.Driver)
                .Include(ds => ds.Shipment)
                .Select(ds => new DriverShipmentDto
                {
                    DriverId = ds.DriverId,
                    ShipmentId = ds.ShipmentId,
                    DriverName = ds.Driver.Name,
                    ShipmentOrigin = ds.Shipment.Origin,
                    ShipmentDestination = ds.Shipment.Destination,
                    ShipmentStatus = ds.Shipment.Status
                })
                .ToListAsync();
        }

        public async Task<DriverShipmentDto?> FindDriverShipment(int driverId, int shipmentId)
        {
            var ds = await _context.DriverShipments
                .Include(ds => ds.Driver)
                .Include(ds => ds.Shipment)
                .FirstOrDefaultAsync(ds => ds.DriverId == driverId && ds.ShipmentId == shipmentId);

            return ds == null ? null : new DriverShipmentDto
            {
                DriverId = ds.DriverId,
                ShipmentId = ds.ShipmentId,
                DriverName = ds.Driver.Name,
                ShipmentOrigin = ds.Shipment.Origin,
                ShipmentDestination = ds.Shipment.Destination,
                ShipmentStatus = ds.Shipment.Status
            };
        }

        public async Task<ServiceResponse> AssignDriverToShipment(int driverId, int shipmentId, string? role = null)
        {
            var exists = await _context.DriverShipments
                .AnyAsync(ds => ds.DriverId == driverId && ds.ShipmentId == shipmentId);

            if (exists)
            {
                return new ServiceResponse
                {
                    Status = ServiceResponse.ServiceStatus.Error,
                    Messages = new List<string> { "Driver is already assigned to this shipment." }
                };
            }

            var driverExists = await _context.Drivers.AnyAsync(d => d.DriverId == driverId);
            var shipmentExists = await _context.Shipments.AnyAsync(s => s.ShipmentId == shipmentId);

            if (!driverExists)
            {
                return new ServiceResponse
                {
                    Status = ServiceResponse.ServiceStatus.NotFound,
                    Messages = new List<string> { "Driver not found." }
                };
            }

            if (!shipmentExists)
            {
                return new ServiceResponse
                {
                    Status = ServiceResponse.ServiceStatus.NotFound,
                    Messages = new List<string> { "Shipment not found." }
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

        public async Task<ServiceResponse> UnassignDriverFromShipment(int driverId, int shipmentId)
        {
            var existing = await _context.DriverShipments
                .FirstOrDefaultAsync(ds => ds.DriverId == driverId && ds.ShipmentId == shipmentId);

            if (existing == null)
            {
                return new ServiceResponse
                {
                    Status = ServiceResponse.ServiceStatus.NotFound,
                    Messages = new List<string> { "Assignment not found." }
                };
            }

            _context.DriverShipments.Remove(existing);
            await _context.SaveChangesAsync();

            return new ServiceResponse { Status = ServiceResponse.ServiceStatus.Deleted };
        }

        public async Task<IEnumerable<DriverDto>> ListDriversForShipment(int shipmentId)
        {
            return await _context.DriverShipments
                .Where(ds => ds.ShipmentId == shipmentId)
                .Include(ds => ds.Driver)
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
                .Include(ds => ds.Shipment)
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

        public Task ListDriverShipmentsForShipment(int id)
        {
            throw new NotImplementedException();
        }
    }
}
