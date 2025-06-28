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
    /// API controller for managing shipment records.
    /// Supports CRUD operations and relationship navigation.
    /// </summary>
    [Route("api/[controller]")] // Consistent Route: /api/Shipments
    [ApiController]
    public class ShipmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipmentsController"/> class.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        public ShipmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the list of all shipments, including their associated truck and drivers.
        /// </summary>
        /// <returns>A list of Shipment objects.</returns>
        /// <response code="200">Returns a list of shipments.</response>
        /// <response code="500">If an unhandled error occurs on the server.</response>
        [HttpGet("list")] // Route: GET /api/Shipments/list (changed to lowercase 'list' for consistency with your provided code)
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Shipment>>> GetShipments() 
        {
            try
            {
                var shipments = await _context.Shipments
                    .Include(s => s.Truck) // Include the Truck navigation property
                                           // .Include(s => s.DriverShipments) 
                                           // .ThenInclude(ds => ds.Driver) 
                    .Select(s => new Shipment
                    {
                        ShipmentId = s.ShipmentId,
                        Origin = s.Origin,
                        Destination = s.Destination,
                        Distance = s.Distance,
                        Status = s.Status,
                        TruckId = s.TruckId,
                    })
                    .ToListAsync();

                return Ok(shipments);
            }
            catch (System.Exception ex)
            {
                // Log the exception ex
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving shipments: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets a specific shipment by ID, including its associated truck and drivers.
        /// </summary>
        /// <param name="id">The unique identifier for the shipment.</param>
        /// <returns>The matching shipment or NotFound.</returns>
        /// <response code="200">Returns the requested shipment.</response>
        /// <response code="404">If the shipment with the given ID is not found.</response>
        /// <response code="500">If an unhandled error occurs on the server.</response>
        [HttpGet("{id}")] // Route: GET /api/Shipments/{id}
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Shipment>> GetShipment(int id)
        {
            try
            {
                var shipment = await _context.Shipments
                    .Include(s => s.Truck)
                    .Include(s => s.DriverShipments)
                        .ThenInclude(ds => ds.Driver)
                    .FirstOrDefaultAsync(s => s.ShipmentId == id);

                if (shipment == null)
                {
                    return NotFound($"Shipment with ID {id} not found.");
                }

                return Ok(shipment);
            }
            catch (System.Exception ex)
            {
                // Log the exception ex
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving shipment with ID {id}: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing shipment.
        /// </summary>
        /// <param name="id">The ID of the shipment to update.</param>
        /// <param name="shipment">The updated shipment object.</param>
        /// <returns>No content if successful, BadRequest, NotFound, or Error.</returns>
        /// <response code="204">The shipment was updated successfully.</response>
        /// <response code="400">If the provided ID does not match the shipment's ID in the body, or the model state is invalid.</response>
        /// <response code="404">If the shipment to be updated is not found.</response>
        /// <response code="500">If an unhandled error occurs during the update process.</response>
        [HttpPut("{id}")] // Route: PUT /api/Shipments/{id}
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutShipment(int id, [FromBody] Shipment shipment)
        {
            if (id != shipment.ShipmentId)
            {
                ModelState.AddModelError(nameof(shipment.ShipmentId), "Shipment ID in body does not match route ID.");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(shipment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShipmentExists(id))
                {
                    return NotFound($"Shipment with ID {id} not found.");
                }
                else
                {
                    throw;
                }
            }
            catch (System.Exception ex)
            {
                // Log the exception ex
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating shipment with ID {id}: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new shipment record.
        /// </summary>
        /// <param name="shipment">The shipment data to create.</param>
        /// <returns>The newly created Shipment object.</returns>
        /// <response code="201">Returns the newly created shipment.</response>
        /// <response code="400">If the shipment data is invalid.</response>
        /// <response code="500">If an unhandled error occurs during creation.</response>
        [HttpPost("Create")] // Route: POST /api/Shipments/Create
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Shipment>> PostShipment([FromBody] Shipment shipment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Shipments.Add(shipment);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetShipment), new { id = shipment.ShipmentId }, shipment);
            }
            catch (DbUpdateException ex)
            {
                // Example: Check for unique constraint violation or other DB-specific errors
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating shipment: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                // Log the exception ex
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating shipment: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a shipment by ID.
        /// </summary>
        /// <param name="id">The ID of the shipment to delete.</param>
        /// <returns>No content if successful, or NotFound.</returns>
        /// <response code="204">The shipment was deleted successfully.</response>
        /// <response code="404">If the shipment to be deleted is not found.</response>
        /// <response code="500">If an unhandled error occurs during deletion.</response>
        [HttpDelete("{id}")] // Route: DELETE /api/Shipments/{id}
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteShipment(int id)
        {
            try
            {
                var shipment = await _context.Shipments.FindAsync(id);
                if (shipment == null)
                {
                    return NotFound($"Shipment with ID {id} not found.");
                }

                _context.Shipments.Remove(shipment);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                // Log the exception ex
                // Check if the error is due to related records (e.g., driver-shipment assignments)
                if (ex.InnerException?.Message?.Contains("FOREIGN KEY constraint failed") == true ||
                    ex.InnerException?.Message?.Contains("Cannot delete or update a parent row: a foreign key constraint fails") == true)
                {
                    return BadRequest($"Cannot delete shipment with ID {id} because it has associated driver assignments. Please remove assignments first.");
                }
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting shipment with ID {id}: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                // Log the exception ex
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting shipment with ID {id}: {ex.Message}");
            }
        }

        private bool ShipmentExists(int id)
        {
            return _context.Shipments.Any(e => e.ShipmentId == id);
        }
    }
}
