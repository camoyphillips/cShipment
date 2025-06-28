using cShipment.Data;
using cShipment.Interfaces;
using cShipment.Models;
using cShipment.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace cShipment.Services
{
    public class DriverService : IDriverService
    {
        private readonly ApplicationDbContext _context;

        public DriverService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DriverDto>> ListDrivers()
        {
            return await _context.Drivers.Select(d => new DriverDto
            {
                DriverId = d.DriverId,
                Name = d.Name,
                LicenseNumber = d.LicenseNumber
            }).ToListAsync();
        }

        public async Task<DriverDto?> FindDriver(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            return driver == null ? null : new DriverDto
            {
                DriverId = driver.DriverId,
                Name = driver.Name,
                LicenseNumber = driver.LicenseNumber
            };
        }

        public async Task<ServiceResponse> AddDriver(DriverDto dto)
        {
            var driver = new Driver
            {
                Name = dto.Name,
                LicenseNumber = dto.LicenseNumber
            };
            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();
            return new ServiceResponse { Status = ServiceResponse.ServiceStatus.Created, CreatedId = driver.DriverId };
        }

        public async Task<ServiceResponse> UpdateDriver(DriverDto dto)
        {
            var driver = await _context.Drivers.FindAsync(dto.DriverId);
            if (driver == null) return new ServiceResponse { Status = ServiceResponse.ServiceStatus.NotFound };
            driver.Name = dto.Name;
            driver.LicenseNumber = dto.LicenseNumber;
            await _context.SaveChangesAsync();
            return new ServiceResponse { Status = ServiceResponse.ServiceStatus.Updated };
        }

        public async Task<ServiceResponse> DeleteDriver(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null) return new ServiceResponse { Status = ServiceResponse.ServiceStatus.NotFound };
            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();
            return new ServiceResponse { Status = ServiceResponse.ServiceStatus.Deleted };
        }

        public Task GetDriverForTruck(int id)
        {
            throw new NotImplementedException();
        }
    }
}