using System; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cShipment.Data;
using cShipment.Models;
using Microsoft.AspNetCore.Http;
using cShipment.Models.Dtos;
using System.IO; 

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
        /// Gets all trucks, including their assigned driver's name.
        /// </summary>
        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TruckDto>>> GetTrucks()
        {
            try
            {
                var trucks = await _context.Trucks
                    .Include(t => t.AssignedDriver)
                    .Select(t => new TruckDto
                    {
                        TruckId = t.TruckId,
                        Model = t.Model,
                        Mileage = (int)t.Mileage, 
                        LastMaintenanceDate = t.LastMaintenanceDate,
                        AssignedDriverId = t.AssignedDriverId, 
                        AssignedDriverName = t.AssignedDriver != null ? t.AssignedDriver.Name : "N/A", 
                        TruckImagePath = t.TruckImagePath
                    })
                    .ToListAsync();

                return Ok(trucks);
            }
            catch (System.Exception ex)
            {
                // TODO: Log the exception
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
                // TODO: Log the exception
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
                // TODO: Log the exception
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
                // TODO: Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating truck: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                // TODO: Log the exception
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
                // TODO: Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting truck with ID {id}: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                // TODO: Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting truck with ID {id}: {ex.Message}");
            }
        }

        /// <summary>
        /// Uploads a truck image file (binary).
        /// </summary>
        /// <param name="id">The ID of the truck to upload the image for.</param>
        /// <param name="TruckPhoto">The image file to upload.</param>
        /// <returns>The TruckId and the new TruckImagePath on success.</returns>
        /// <response code="200">Returns the truck ID and the new image path.</response>
        /// <response code="400">If no file is uploaded or file is empty.</response>
        /// <response code="404">If the truck with the given ID is not found.</response>
        /// <response code="500">If an error occurs during file upload or database update.</response>
        [HttpPost("UploadImage/{id}")] // Corrected route for binary upload
        [Consumes("multipart/form-data")] // Important for file uploads
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadTruckImage(int id, IFormFile TruckPhoto)
        {
            if (TruckPhoto == null || TruckPhoto.Length == 0)
            {
                return BadRequest("No file uploaded or file is empty.");
            }

            try
            {
                var truck = await _context.Trucks.FindAsync(id);
                if (truck == null)
                {
                    return NotFound($"Truck with ID {id} not found.");
                }

                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "trucks");
                // Ensure the directory exists; CreateDirectory is idempotent (doesn't fail if exists)
                Directory.CreateDirectory(uploadsPath);

                // Create a unique file name to prevent conflicts
                // Using Guid.NewGuid() ensures uniqueness. Keep original extension.
                var fileName = $"truck_{id}_{Guid.NewGuid()}{Path.GetExtension(TruckPhoto.FileName)}";
                var fullPath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await TruckPhoto.CopyToAsync(stream);
                }

                // Update the truck's image path in the database
                truck.TruckImagePath = $"/uploads/trucks/{fileName}";
                await _context.SaveChangesAsync();

                // Return relevant info
                return Ok(new { truck.TruckId, truck.TruckImagePath });
            }
            catch (Exception ex)
            {
                // TODO: Log the exception (e.g., using ILogger)
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred during file upload or database update for truck ID {id}: {ex.Message}");
            }
        }


        private bool TruckExists(int id) => _context.Trucks.Any(t => t.TruckId == id);
    }
}