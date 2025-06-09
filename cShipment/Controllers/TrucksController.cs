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
    /// API controller for managing trucks in the shipment system.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TrucksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TrucksController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all trucks, including their assigned driver and shipments.
        /// </summary>
        [HttpGet("List")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Truck>>> GetTrucks()
        {
            try
            {
                return Ok(await _context.Trucks
                    .Include(t => t.AssignedDriver)
                    .Include(t => t.Shipments)
                    .ToListAsync());
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving trucks: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets a truck by ID, including related driver and shipments.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Truck>> GetTruck(int id)
        {
            try
            {
                var truck = await _context.Trucks
                    .Include(t => t.AssignedDriver)
                    .Include(t => t.Shipments)
                    .FirstOrDefaultAsync(t => t.TruckId == id);

                if (truck == null)
                    return NotFound($"Truck with ID {id} not found.");

                return Ok(truck);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving truck with ID {id}: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing truck.
        /// </summary>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutTruck(int id, [FromBody] Truck truck)
        {
            if (id != truck.TruckId)
            {
                ModelState.AddModelError(nameof(truck.TruckId), "Truck ID mismatch.");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Entry(truck).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TruckExists(id))
                    return NotFound($"Truck with ID {id} not found.");
                else
                    throw;
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating truck with ID {id}: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new truck.
        /// </summary>
        [HttpPost("Create")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Truck>> PostTruck([FromBody] Truck truck)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _context.Trucks.Add(truck);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTruck), new { id = truck.TruckId }, truck);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message?.Contains("Duplicate entry") == true)
                {
                    ModelState.AddModelError("Truck", "A truck with this unique constraint already exists.");
                    return BadRequest(ModelState);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating truck: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating truck: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a truck by ID.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTruck(int id)
        {
            try
            {
                var truck = await _context.Trucks.FindAsync(id);
                if (truck == null)
                    return NotFound($"Truck with ID {id} not found.");

                _context.Trucks.Remove(truck);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message?.Contains("FOREIGN KEY constraint") == true)
                {
                    return BadRequest($"Cannot delete truck with ID {id} because it is referenced by other records.");
                }
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting truck with ID {id}: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting truck with ID {id}: {ex.Message}");
            }
        }

        private bool TruckExists(int id) =>
            _context.Trucks.Any(t => t.TruckId == id);
    }
}
