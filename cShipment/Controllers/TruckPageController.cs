using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using cShipment.Interfaces;
using cShipment.Models;
using cShipment.Models.Dtos;
using cShipment.Models.ViewModels;

namespace cShipment.Controllers
{
    /// <summary>
    /// Handles Truck page logic and views for Razor Pages.
    /// This controller manages the UI interactions related to trucks,
    /// including listing, viewing details, adding, editing, and deleting trucks.
    /// It interacts with service layers for data operations.
    /// </summary>
    public class TruckPageController(ITruckService truckService) : Controller // Primary constructor used here
    {
        private readonly ITruckService _truckService = truckService;

        // IDriverService was previously here but removed based on a recent request for a
        // simplified Details method.
        // If you need it for other actions (e.g., assigning a driver during New/Edit), you'll need to
        // re-add it
        // both to the constructor and as a private readonly field.

        /// <summary>
        /// Redirects the base URL for TruckPage to the List action.
        /// </summary>
        /// <returns>A redirect to the List action.</returns>
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        /// <summary>
        /// Displays a list of all trucks.
        /// </summary>
        /// <returns>A View containing a collection of Truck DTOs.</returns>
        // GET: TruckPage/List
        public async Task<IActionResult> List()
        {
            try
            {
                var trucks = await _truckService.ListTrucks();
                return View(trucks);
            }
            catch (Exception ex)
            {
                // TODO: Log the exception (e.g., using an injected ILogger)
                // Console.WriteLine($"Error retrieving truck list: {ex.Message}");
                return View("Error", new ErrorViewModel { Messages = { "An error occurred while loading the truck list.", ex.Message } }); // Simplified collection initialization
            }
        }

        /// <summary>
        /// Displays the detailed information for a specific truck.
        /// This action now directly passes a TruckDto to the view.
        /// </summary>
        /// <param name="id">The ID of the truck to display details for.</param>
        /// <returns>A View with TruckDto, or an Error View if the truck is not found or an error occurs.</returns>
        // GET: /TruckPage/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var truck = await _truckService.FindTruck(id);
                if (truck == null)
                {
                    // If truck not found, return an error view with a specific message
                    return View("Error", new ErrorViewModel
                    {
                        Messages = { $"Truck with ID {id} not found." } // Simplified collection initialization
                    });
                }

                // IMPORTANT: The 'Details.cshtml' view should now expect a 'TruckDto' directly.
                // It will no longer have direct access to AssignedDriver (as a separate object),
                // ActiveShipments, or MaintenanceLogs collections through this model.
                // If those are needed, either update the TruckDto to include them,
                // or revert to a more complex ViewModel (e.g., TruckDetails) and fetch them here.
                return View("Details", truck);
            }
            catch (Exception ex)
            {
                // TODO: Log the exception
                // Console.WriteLine($"Error retrieving truck details for ID {id}: {ex.Message}");
                return View("Error", new ErrorViewModel { Messages = { $"An error occurred while loading details for truck ID {id}.", ex.Message } }); // Simplified collection initialization
            }
        }

        /// <summary>
        /// Displays the form for creating a new truck.
        /// </summary>
        /// <returns>The New Truck View.</returns>
        // GET: TruckPage/New
        public IActionResult New()
        {
            return View();
        }

        /// <summary>
        /// Handles the POST request for adding a new truck.
        /// </summary>
        /// <param name="truckDto">The TruckDto containing the new truck's data.</param>
        /// <returns>Redirects to details on success, or an Error View on failure.</returns>
        // POST: TruckPage/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TruckDto truckDto)
        {
            if (!ModelState.IsValid)
            {
                // If model validation fails, return the view with validation errors
                return View("New", truckDto); // Pass the DTO back to the view to retain input
            }

            try
            {
                var response = await _truckService.AddTruck(truckDto);
                if (response.Status == ServiceResponse.ServiceStatus.Created && response.CreatedId.HasValue)
                {
                    TempData["SuccessMessage"] = "Truck added successfully!";
                    // Redirect to the Details page of the newly created truck
                    return RedirectToAction("Details", new { id = response.CreatedId.Value });
                }
                // If status is not Created, there was an error or bad request from the service layer
                return View("Error", new ErrorViewModel { Messages = response.Messages });
            }
            catch (Exception ex)
            {
                // TODO: Log the exception
                // Console.WriteLine($"Error adding new truck: {ex.Message}");
                return View("Error", new ErrorViewModel { Messages = { "An unexpected error occurred while adding the truck.", ex.Message } }); // Simplified collection initialization
            }
        }

        /// <summary>
        /// Displays the form for editing an existing truck.
        /// </summary>
        /// <param name="id">The ID of the truck to edit.</param>
        /// <returns>The Edit Truck View, or an Error View if the truck is not found.</returns>
        // GET: TruckPage/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var truck = await _truckService.FindTruck(id);
                // If truck is null, return an error view; otherwise, return the edit view with the truck data
                return truck == null ? View("Error", new ErrorViewModel { Messages = { $"Truck with ID {id} not found for editing." } }) : View(truck); // Simplified collection initialization
            }
            catch (Exception ex)
            {
                // TODO: Log the exception
                // Console.WriteLine($"Error retrieving truck for editing (ID {id}): {ex.Message}");
                return View("Error", new ErrorViewModel { Messages = { $"An error occurred while preparing to edit truck ID {id}.", ex.Message } }); // Simplified collection initialization
            }
        }

        /// <summary>
        /// Handles the POST request for updating an existing truck.
        /// Includes logic for uploading a new truck photo.
        /// </summary>
        /// <param name="id">The ID of the truck being updated (from route).</param>
        /// <param name="truckDto">The TruckDto containing the updated data.</param>
        /// <param name="TruckPhoto">Optional: The new photo file for the truck.</param>
        /// <returns>Redirects to details on success, or an Error View on failure.</returns>
        // POST: TruckPage/Update/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, TruckDto truckDto, IFormFile? TruckPhoto)
        {
            if (id != truckDto.TruckId)
            {
                // Mismatch between route ID and DTO ID indicates a bad request
                return View("Error", new ErrorViewModel { Messages = { "Truck ID mismatch during update." } }); // Simplified collection initialization
            }

            if (!ModelState.IsValid)
            {
                // If model validation fails, return to the edit view with validation errors
                return View("Edit", truckDto);
            }

            try
            {
                // Handle file upload if a new photo is provided
                if (TruckPhoto != null && TruckPhoto.Length > 0)
                {
                    // Define the path where images will be saved within wwwroot
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "trucks");
                    // Ensure the directory exists; CreateDirectory is idempotent (does nothing if folder already exists)
                    Directory.CreateDirectory(uploadsFolder);

                    // Generate a unique file name to prevent overwrites and ensure clean URLs
                    // Appending Guid.NewGuid() makes the filename highly unique
                    string fileName = $"truck_{id}_{Guid.NewGuid()}{Path.GetExtension(TruckPhoto.FileName)}";
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    // Save the file to the determined physical path
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await TruckPhoto.CopyToAsync(stream);
                    }

                    // Store the web-accessible path in the DTO
                    // This path is relative to wwwroot and will be saved to the database
                    truckDto.TruckImagePath = $"/uploads/trucks/{fileName}";
                }

                var response = await _truckService.UpdateTruck(truckDto);

                if (response.Status == ServiceResponse.ServiceStatus.Updated)
                {
                    TempData["SuccessMessage"] = "Truck updated successfully!";
                    // Redirect to the Details page of the updated truck
                    return RedirectToAction("Details", new { id = truckDto.TruckId });
                }
                // If update failed (e.g., NotFound from service, or other service-level error)
                return View("Error", new ErrorViewModel { Messages = response.Messages });
            }
            catch (Exception ex)
            {
                // TODO: Log the exception
                // Console.WriteLine($"Error updating truck ID {id}: {ex.Message}");
                return View("Error", new ErrorViewModel { Messages = { $"An unexpected error occurred while updating truck ID {id}.", ex.Message } }); // Simplified collection initialization
            }
        }

        /// <summary>
        /// Displays a confirmation page before deleting a truck.
        /// </summary>
        /// <param name="id">The ID of the truck to confirm deletion for.</param>
        /// <returns>The Confirm Delete View, or an Error View if the truck is not found.</returns>
        // GET: TruckPage/ConfirmDelete/{id}
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                var truck = await _truckService.FindTruck(id);
                // If truck is null, return an error view; otherwise, return the truck for confirmation
                return truck == null ? View("Error", new ErrorViewModel { Messages = { $"Truck with ID {id} not found for deletion confirmation." } }) : View(truck); // Simplified collection initialization
            }
            catch (Exception ex)
            {
                // TODO: Log the exception
                // Console.WriteLine($"Error retrieving truck for delete confirmation (ID {id}): {ex.Message}");
                return View("Error", new ErrorViewModel { Messages = { $"An error occurred while preparing for truck deletion (ID {id}).", ex.Message } }); // Simplified collection initialization
            }
        }

        /// <summary>
        /// Handles the POST request for deleting a truck.
        /// </summary>
        /// <param name="id">The ID of the truck to delete.</param>
        /// <returns>Redirects to the List view on success, or an Error View on failure.</returns>
        // POST: TruckPage/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _truckService.DeleteTruck(id);
                if (response.Status == ServiceResponse.ServiceStatus.Deleted)
                {
                    TempData["SuccessMessage"] = "Truck deleted successfully!";
                    // Redirect to the list of trucks after successful deletion
                    return RedirectToAction("List");
                }
                // If deletion failed (e.g., NotFound from service, or other service-level error)
                return View("Error", new ErrorViewModel { Messages = response.Messages });
            }
            catch (Exception ex)
            {
                // TODO: Log the exception
                // Console.WriteLine($"Error deleting truck ID {id}: {ex.Message}");
                return View("Error", new ErrorViewModel { Messages = { $"An unexpected error occurred while deleting truck ID {id}.", ex.Message } }); // Simplified collection initialization
            }
        }
    }
}