using cShipment.Models;
using cShipment.Models.Dtos;

namespace cShipment.Interfaces
{
    /// <summary>
    /// Defines the contract for driver-related business logic operations.
    /// </summary>
    public interface IDriverService
    {
        /// <summary>
        /// Retrieves a list of all drivers.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, yielding an enumerable of <see cref="DriverDto"/>.</returns>
        Task<IEnumerable<DriverDto>> ListDrivers();

        /// <summary>
        /// Finds a specific driver by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the driver.</param>
        /// <returns>A task representing the asynchronous operation, yielding the <see cref="DriverDto"/> if found, otherwise null.</returns>
        Task<DriverDto?> FindDriver(int id);

        /// <summary>
        /// Adds a new driver to the system.
        /// </summary>
        /// <param name="driverDto">The <see cref="DriverDto"/> containing the data for the new driver.</param>
        /// <returns>A task representing the asynchronous operation, yielding a <see cref="ServiceResponse"/> indicating the outcome.</returns>
        Task<ServiceResponse> AddDriver(DriverDto driverDto);

        /// <summary>
        /// Updates an existing driver's details.
        /// </summary>
        /// <param name="driverDto">The <see cref="DriverDto"/> containing the updated data for the driver.</param>
        /// <returns>A task representing the asynchronous operation, yielding a <see cref="ServiceResponse"/> indicating the outcome.</returns>
        Task<ServiceResponse> UpdateDriver(DriverDto driverDto);

        /// <summary>
        /// Deletes a driver from the system by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the driver to delete.</param>
        /// <returns>A task representing the asynchronous operation, yielding a <see cref="ServiceResponse"/> indicating the outcome.</returns>
        Task<ServiceResponse> DeleteDriver(int id);
    }
}