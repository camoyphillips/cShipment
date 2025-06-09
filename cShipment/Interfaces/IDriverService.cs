using cShipment.Models;
using cShipment.Models.Dtos;

namespace cShipment.Interfaces
{
    public interface IDriverService
    {
        Task<IEnumerable<DriverDto>> ListDrivers();

        Task<DriverDto?> FindDriver(int id);

        Task<ServiceResponse> AddDriver(DriverDto dto);

        Task<ServiceResponse> UpdateDriver(DriverDto dto);

        Task<ServiceResponse> DeleteDriver(int id);
    }
}
