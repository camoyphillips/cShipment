using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cShipment.Data;
using cShipment.Models;
using Microsoft.AspNetCore.Http; 

namespace cShipment.Controllers
{
    /// <summary>
    /// API controller for managing truck drivers in the shipment system.
    /// </summary>
    [Route("api/[controller]")] // Consistent Route: /api/Drivers
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DriversController"/> class.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        public DriversController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all drivers along with their assigned shipments.
        /// </summary>
        /// <returns>A list of Driver objects including associated shipments.</returns>
        /// <response code="200">Returns a list of drivers.</response>
        /// <response code="500">If an unhandled error occurs on the server.</response>
        [HttpGet("List")] // Route: GET /api/Drivers/List
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Driver>>> GetDrivers()
        {
            try
            {
                return Ok(await _context.Drivers
                    .Include(d => d.DriverShipments)
                        .ThenInclude(ds => ds.Shipment)
                    .ToListAsync());
            }
            catch (System.Exception ex)
            {
                // Log the exception ex
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving drivers: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets a driver by their ID, including assigned shipments.
        /// </summary>
        /// <param name="id">The unique identifier for the driver.</param>
        /// <returns>A Driver object or NotFound.</returns>
        /// <response code="200">Returns the requested driver.</response>
        /// <response code="404">If the driver with the given ID is not found.</response>
        /// <response code="500">If an unhandled error occurs on the server.</response>
        [HttpGet("{id}")] // Route: GET /api/Drivers/{id}
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Driver>> GetDriver(int id)
        {
            try
            {
                var driver = await _context.Drivers
                    .Include(d => d.DriverShipments)
                        .ThenInclude(ds => ds.Shipment)
                    .FirstOrDefaultAsync(d => d.DriverId == id);

                if (driver == null)
                {
                    return NotFound($"Driver with ID {id} not found.");
                }

                return Ok(driver);
            }
            catch (System.Exception ex)
            {
                // Log the exception ex
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving driver with ID {id}: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing driver.
        /// </summary>
        /// <param name="id">The ID of the driver to update.</param>
        /// <param name="driver">The updated driver object.</param>
        /// <returns>No content if successful, BadRequest, NotFound, or Error.</returns>
        /// <response code="204">The driver was updated successfully.</response>
        /// <response code="400">If the provided ID does not match the driver's ID in the body, or the model state is invalid.</response>
        /// <response code="404">If the driver to be updated is not found.</response>
        /// <response code="500">If an unhandled error occurs during the update process.</response>
        [HttpPut("{id}")] // Route: PUT /api/Drivers/{id}
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutDriver(int id, [FromBody] Driver driver)
        {
            if (id != driver.DriverId)
            {
                ModelState.AddModelError(nameof(driver.DriverId), "Driver ID in body does not match route ID.");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(driver).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverExists(id))
                {
                    return NotFound($"Driver with ID {id} not found.");
                }
                else
                {
                    // This typically means another process modified or deleted the entity
                    // before this update was applied. Re-throw for general error handling.
                    throw;
                }
            }
            catch (System.Exception ex)
            {
                // Log the exception ex
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating driver with ID {id}: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new driver.
        /// </summary>
        /// <param name="driver">The driver object to create.</param>
        /// <returns>The newly created Driver object.</returns>
        /// <response code="201">Returns the newly created driver.</response>
        /// <response code="400">If the driver data is invalid.</response>
        /// <response code="500">If an unhandled error occurs during creation.</response>
        [HttpPost("Create")] // Route: POST /api/Drivers/Create
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Driver>> PostDriver([FromBody] Driver driver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Drivers.Add(driver);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetDriver), new { id = driver.DriverId }, driver);
            }
            catch (DbUpdateException ex) // Catch database-specific exceptions
            {
                // Example: Check for unique constraint violation on LicenseNumber
                if (ex.InnerException?.Message?.Contains("Duplicate entry") == true) // Adjust for specific DB error message
                {
                    ModelState.AddModelError(nameof(driver.LicenseNumber), "Driver with this License Number already exists.");
                    return BadRequest(ModelState);
                }
                // Log the exception ex
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating driver: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                // Log the exception ex
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating driver: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a driver by ID.
        /// </summary>
        /// <param name="id">The ID of the driver to delete.</param>
        /// <returns>No content if successful, or NotFound.</returns>
        /// <response code="204">The driver was deleted successfully.</response>
        /// <response code="404">If the driver to be deleted is not found.</response>
        /// <response code="500">If an unhandled error occurs during deletion.</response>
        [HttpDelete("{id}")] // Route: DELETE /api/Drivers/{id}
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            try
            {
                var driver = await _context.Drivers.FindAsync(id);
                if (driver == null)
                {
                    return NotFound($"Driver with ID {id} not found.");
                }

                _context.Drivers.Remove(driver);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex) // Catch potential FK constraint errors
            {
                // Log the exception ex
                // Example: Check if the error is due to related records (e.g., driver assigned to shipments)
                if (ex.InnerException?.Message?.Contains("FOREIGN KEY constraint failed") == true ||
                    ex.InnerException?.Message?.Contains("Cannot delete or update a parent row: a foreign key constraint fails") == true)
                {
                    return BadRequest($"Cannot delete driver with ID {id} because there are associated shipments. Please unassign shipments first.");
                }
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting driver with ID {id}: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                // Log the exception ex
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting driver with ID {id}: {ex.Message}");
            }
        }

        private bool DriverExists(int id)
        {
            return _context.Drivers.Any(e => e.DriverId == id);
        }
    }
}