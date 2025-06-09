using cShipment.Data;
using cShipment.Interfaces;
using cShipment.Models;
using cShipment.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace cShipment.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly ApplicationDbContext _context;

        public ShipmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ShipmentDto>> ListShipments()
        {
            var shipments = await _context.Shipments.Include(s => s.Truck).ToListAsync();
            return shipments.Select(s => new ShipmentDto
            {
                ShipmentId = s.ShipmentId,
                Origin = s.Origin,
                Destination = s.Destination,
                Distance = s.Distance,
                Status = s.Status,
                TruckId = s.TruckId
            });
        }

        public async Task<ShipmentDto?> FindShipment(int id)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            return shipment == null ? null : new ShipmentDto
            {
                ShipmentId = shipment.ShipmentId,
                Origin = shipment.Origin,
                Destination = shipment.Destination,
                Distance = shipment.Distance,
                Status = shipment.Status,
                TruckId = shipment.TruckId
            };
        }

        public async Task<ServiceResponse> AddShipment(ShipmentDto shipmentDto)
        {
            var shipment = new Shipment
            {
                Origin = shipmentDto.Origin,
                Destination = shipmentDto.Destination,
                Distance = shipmentDto.Distance,
                Status = shipmentDto.Status,
                TruckId = shipmentDto.TruckId
            };

            _context.Shipments.Add(shipment);
            await _context.SaveChangesAsync();

            return new ServiceResponse { Status = ServiceResponse.ServiceStatus.Created, CreatedId = shipment.ShipmentId };
        }

        public async Task<ServiceResponse> UpdateShipment(ShipmentDto shipmentDto)
        {
            var shipment = await _context.Shipments.FindAsync(shipmentDto.ShipmentId);
            if (shipment == null)
                return new ServiceResponse { Status = ServiceResponse.ServiceStatus.NotFound };

            shipment.Origin = shipmentDto.Origin;
            shipment.Destination = shipmentDto.Destination;
            shipment.Distance = shipmentDto.Distance;
            shipment.Status = shipmentDto.Status;
            shipment.TruckId = shipmentDto.TruckId;

            _context.Entry(shipment).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return new ServiceResponse { Status = ServiceResponse.ServiceStatus.Updated };
        }

        public async Task<ServiceResponse> DeleteShipment(int id)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment == null)
                return new ServiceResponse { Status = ServiceResponse.ServiceStatus.NotFound };

            _context.Shipments.Remove(shipment);
            await _context.SaveChangesAsync();

            return new ServiceResponse { Status = ServiceResponse.ServiceStatus.Deleted };
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

        public async Task<IEnumerable<ShipmentDto>> ListShipmentsForTruck(int truckId)
        {
            return await _context.Shipments
                .Where(s => s.TruckId == truckId)
                .Select(s => new ShipmentDto
                {
                    ShipmentId = s.ShipmentId,
                    Origin = s.Origin,
                    Destination = s.Destination,
                    Distance = s.Distance,
                    Status = s.Status,
                    TruckId = s.TruckId
                }).ToListAsync();
        }
    }
}
