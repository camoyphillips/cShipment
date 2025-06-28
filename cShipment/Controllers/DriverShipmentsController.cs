using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cShipment.Data;
using cShipment.Models;
using cShipment.Models.Dtos;
using Microsoft.AspNetCore.Http;

namespace cShipment.Controllers
{
    /// <summary>
    /// API controller for managing driver-shipment assignments.
    /// Provides detailed views and CRUD operations for associating drivers with shipments.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DriverShipmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverShipmentsController"/> class.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        public DriverShipmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a list of all driver-shipment assignments, including details about the driver and the shipment.
        /// </summary>
        /// <returns>A list of <see cref="DriverShipmentDto"/> objects.</returns>
        /// <response code="200">Returns a list of driver-shipment assignments.</response>
        /// <response code="500">If an unhandled error occurs on the server.</response>
        [HttpGet("list")] // Route: GET /api/DriverShipments/list
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DriverShipmentDto>>> GetDriverShipments()
        {
            try
            {
                var driverShipments = await _context.DriverShipments
                    .Include(ds => ds.Driver)     // Include the Driver navigation property
                    .Include(ds => ds.Shipment)   // Include the Shipment navigation property
                    .Select(ds => new DriverShipmentDto // Project to DriverShipmentDto
                    {
                        DriverShipmentId = ds.DriverShipmentId,
                        DriverId = ds.DriverId,
                        DriverName = ds.Driver.Name, // Access driver's name
                        ShipmentId = ds.ShipmentId,
                        ShipmentOrigin = ds.Shipment.Origin, // Access shipment's origin
                        ShipmentDestination = ds.Shipment.Destination, // Access shipment's destination
                        Role = ds.Role,
                        AssignedOn = ds.AssignedOn
                    })
                    .ToListAsync();

                return Ok(driverShipments);
            }
            catch (System.Exception ex)
            {
                // Log the exception (e.g., using ILogger)
                Console.WriteLine($"Error in GetDriverShipments: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving driver shipments: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets a specific driver-shipment assignment by its unique ID.
        /// </summary>
        /// <param name="id">The unique identifier for the driver-shipment assignment.</param>
        /// <returns>The matching <see cref="DriverShipmentDto"/> or NotFound.</returns>
        /// <response code="200">Returns the requested driver-shipment assignment.</response>
        /// <response code="404">If the assignment with the given ID is not found.</response>
        /// <response code="500">If an unhandled error occurs on the server.</response>
        [HttpGet("{id}")] // Route: GET /api/DriverShipments/{id}
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DriverShipmentDto>> GetDriverShipment(int id)
        {
            try
            {
                var driverShipment = await _context.DriverShipments
                    .Include(ds => ds.Driver)
                    .Include(ds => ds.Shipment)
                    .FirstOrDefaultAsync(ds => ds.DriverShipmentId == id);

                if (driverShipment == null)
                {
                    return NotFound($"Driver-Shipment assignment with ID {id} not found.");
                }

                // Map to DTO for response
                var dto = new DriverShipmentDto
                {
                    DriverShipmentId = driverShipment.DriverShipmentId,
                    DriverId = driverShipment.DriverId,
                    DriverName = driverShipment.Driver.Name,
                    ShipmentId = driverShipment.ShipmentId,
                    ShipmentOrigin = driverShipment.Shipment.Origin,
                    ShipmentDestination = driverShipment.Shipment.Destination,
                    Role = driverShipment.Role,
                    AssignedOn = driverShipment.AssignedOn
                };

                return Ok(dto);
            }
            catch (System.Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in GetDriverShipment (ID: {id}): {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving driver-shipment with ID {id}: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new driver-shipment assignment.
        /// </summary>
        /// <param name="driverShipmentDto">The DTO containing the driver-shipment assignment data to create.</param>
        /// <returns>The newly created <see cref="DriverShipmentDto"/> object.</returns>
        /// <response code="201">Returns the newly created assignment.</response>
        /// <response code="400">If the assignment data is invalid, or if it violates a unique constraint (e.g., duplicate driver-shipment pair).</response>
        /// <response code="500">If an unhandled error occurs during creation.</response>
        [HttpPost("Create")] // Route: POST /api/DriverShipments/Create
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DriverShipmentDto>> PostDriverShipment([FromBody] DriverShipmentDto driverShipmentDto)
        {
            // Clear any ID from the DTO, as the database will generate it
            driverShipmentDto.DriverShipmentId = null;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the assignment already exists based on DriverId and ShipmentId
            if (await _context.DriverShipments.AnyAsync(ds => ds.DriverId == driverShipmentDto.DriverId && ds.ShipmentId == driverShipmentDto.ShipmentId))
            {
                ModelState.AddModelError("", "This driver is already assigned to this shipment.");
                return BadRequest(ModelState);
            }

            try
            {
                var driverShipment = new DriverShipment
                {
                    DriverId = driverShipmentDto.DriverId,
                    ShipmentId = driverShipmentDto.ShipmentId,
                    Role = driverShipmentDto.Role,
                    AssignedOn = driverShipmentDto.AssignedOn == default ? DateTime.UtcNow : driverShipmentDto.AssignedOn // Use UTC Now if not provided
                };

                _context.DriverShipments.Add(driverShipment);
                await _context.SaveChangesAsync();

                // To return the full DTO with DriverName and Shipment details,
                // we need to re-query or manually populate after save.
                // For simplicity, we'll return a DTO with the generated ID and input values.
                driverShipmentDto.DriverShipmentId = driverShipment.DriverShipmentId;

                // Optionally, fetch full details for the response
                var createdDto = await GetDriverShipment(driverShipment.DriverShipmentId);
                if (createdDto.Result is OkObjectResult okResult && okResult.Value is DriverShipmentDto populatedDto)
                {
                    return CreatedAtAction(nameof(GetDriverShipment), new { id = populatedDto.DriverShipmentId }, populatedDto);
                }
                else
                {
                    // Fallback if detailed fetch fails, return with just input data + ID
                    return CreatedAtAction(nameof(GetDriverShipment), new { id = driverShipment.DriverShipmentId }, driverShipmentDto);
                }
            }
            catch (DbUpdateException ex)
            {
                // Log the exception
                Console.WriteLine($"Error in PostDriverShipment (DbUpdateException): {ex.Message}");
                if (ex.InnerException?.Message?.Contains("FOREIGN KEY constraint") == true ||
                    ex.InnerException?.Message?.Contains("The INSERT statement conflicted with the FOREIGN KEY constraint") == true)
                {
                    ModelState.AddModelError("", "Invalid DriverId or ShipmentId provided. Ensure they exist in the database.");
                    return BadRequest(ModelState);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating driver-shipment assignment: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in PostDriverShipment: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating driver-shipment assignment: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing driver-shipment assignment.
        /// </summary>
        /// <param name="id">The ID of the assignment to update.</param>
        /// <param name="driverShipmentDto">The updated assignment data.</param>
        /// <returns>No content if successful, or NotFound/BadRequest.</returns>
        /// <response code="204">The assignment was updated successfully.</response>
        /// <response code="400">If the ID in the route does not match the DTO, or if the data is invalid.</response>
        /// <response code="404">If the assignment to be updated is not found.</response>
        /// <response code="500">If an unhandled error occurs during update.</response>
        [HttpPut("{id}")] // Route: PUT /api/DriverShipments/{id}
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutDriverShipment(int id, [FromBody] DriverShipmentDto driverShipmentDto)
        {
            if (id != driverShipmentDto.DriverShipmentId)
            {
                return BadRequest("DriverShipmentId mismatch between route and body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var driverShipment = await _context.DriverShipments.FindAsync(id);
            if (driverShipment == null)
            {
                return NotFound($"Driver-Shipment assignment with ID {id} not found.");
            }

            // Check if attempting to change DriverId or ShipmentId to an existing pair
            if (driverShipment.DriverId != driverShipmentDto.DriverId || driverShipment.ShipmentId != driverShipmentDto.ShipmentId)
            {
                if (await _context.DriverShipments.AnyAsync(ds => ds.DriverId == driverShipmentDto.DriverId && ds.ShipmentId == driverShipmentDto.ShipmentId && ds.DriverShipmentId != id))
                {
                    ModelState.AddModelError("", "Changing DriverId or ShipmentId would create a duplicate assignment.");
                    return BadRequest(ModelState);
                }
            }

            // Validate if DriverId and ShipmentId actually exist if they are being changed
            if (driverShipment.DriverId != driverShipmentDto.DriverId)
            {
                if (!await _context.Drivers.AnyAsync(d => d.DriverId == driverShipmentDto.DriverId))
                {
                    ModelState.AddModelError(nameof(driverShipmentDto.DriverId), "Invalid Driver ID provided.");
                    return BadRequest(ModelState);
                }
            }

            if (driverShipment.ShipmentId != driverShipmentDto.ShipmentId)
            {
                if (!await _context.Shipments.AnyAsync(s => s.ShipmentId == driverShipmentDto.ShipmentId))
                {
                    ModelState.AddModelError(nameof(driverShipmentDto.ShipmentId), "Invalid Shipment ID provided.");
                    return BadRequest(ModelState);
                }
            }


            // Update properties
            driverShipment.DriverId = driverShipmentDto.DriverId;
            driverShipment.ShipmentId = driverShipmentDto.ShipmentId;
            driverShipment.Role = driverShipmentDto.Role;
            driverShipment.AssignedOn = driverShipmentDto.AssignedOn; // Allow updating assigned date

            _context.Entry(driverShipment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverShipmentExists(id))
                {
                    return NotFound($"Driver-Shipment assignment with ID {id} not found.");
                }
                else
                {
                    throw; // Re-throw if it's a true concurrency issue
                }
            }
            catch (DbUpdateException ex)
            {
                // Log the exception
                Console.WriteLine($"Error in PutDriverShipment (DbUpdateException): {ex.Message}");
                if (ex.InnerException?.Message?.Contains("FOREIGN KEY constraint") == true ||
                    ex.InnerException?.Message?.Contains("The UPDATE statement conflicted with the FOREIGN KEY constraint") == true)
                {
                    ModelState.AddModelError("", "Invalid DriverId or ShipmentId provided. Ensure they exist in the database.");
                    return BadRequest(ModelState);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating driver-shipment assignment with ID {id}: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in PutDriverShipment: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating driver-shipment assignment with ID {id}: {ex.Message}");
            }

            return NoContent(); // 204 No Content indicates successful update
        }


        /// <summary>
        /// Deletes a driver-shipment assignment by ID.
        /// </summary>
        /// <param name="id">The ID of the assignment to delete.</param>
        /// <returns>No content if successful, or NotFound.</returns>
        /// <response code="204">The assignment was deleted successfully.</response>
        /// <response code="404">If the assignment to be deleted is not found.</response>
        /// <response code="500">If an unhandled error occurs during deletion.</response>
        [HttpDelete("{id}")] // Route: DELETE /api/DriverShipments/{id}
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteDriverShipment(int id)
        {
            try
            {
                var driverShipment = await _context.DriverShipments.FindAsync(id);
                if (driverShipment == null)
                {
                    return NotFound($"Driver-Shipment assignment with ID {id} not found.");
                }

                _context.DriverShipments.Remove(driverShipment);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                // Log the exception
                Console.WriteLine($"Error in DeleteDriverShipment (DbUpdateException): {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting driver-shipment assignment with ID {id}: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in DeleteDriverShipment: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting driver-shipment assignment with ID {id}: {ex.Message}");
            }
        }

        /// <summary>
        /// Helper method to check if a DriverShipment exists by its ID.
        /// </summary>
        /// <param name="id">The ID of the DriverShipment.</param>
        /// <returns>True if the DriverShipment exists, false otherwise.</returns>
        private bool DriverShipmentExists(int id)
        {
            return _context.DriverShipments.Any(e => e.DriverShipmentId == id);
        }
    }
}