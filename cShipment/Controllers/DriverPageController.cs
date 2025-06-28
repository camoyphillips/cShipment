using System;
using System.Collections.Generic;
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
    /// Handles Driver page logic and views for Razor Pages.
    /// This controller manages listing, viewing details, adding, editing, and deleting drivers.
    /// It interacts with the IDriverService for all data operations.
    /// </summary>
    public class DriverPageController : Controller
    {
        private readonly IDriverService _driverService;

        public DriverPageController(IDriverService driverService)
        {
            _driverService = driverService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public async Task<IActionResult> List()
        {
            try
            {
                var drivers = await _driverService.ListDrivers();
                return View(drivers);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Messages = new List<string> { "Failed to load driver list.", ex.Message } });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var driver = await _driverService.FindDriver(id);
                if (driver == null)
                {
                    return View("Error", new ErrorViewModel { Messages = new List<string> { $"Driver with ID {id} not found." } });
                }

                return View("Details", driver);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Messages = new List<string> { $"Error loading details for driver ID {id}.", ex.Message } });
            }
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(DriverDto driverDto)
        {
            if (!ModelState.IsValid)
            {
                return View("New", driverDto);
            }

            try
            {
                var response = await _driverService.AddDriver(driverDto);
                if (response.Status == ServiceResponse.ServiceStatus.Created && response.CreatedId.HasValue)
                {
                    TempData["SuccessMessage"] = "Driver added successfully!";
                    return RedirectToAction("Details", new { id = response.CreatedId.Value });
                }

                return View("Error", new ErrorViewModel { Messages = response.Messages });
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Messages = new List<string> { "An unexpected error occurred while adding the driver.", ex.Message } });
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var driver = await _driverService.FindDriver(id);
                if (driver == null)
                {
                    return View("Error", new ErrorViewModel { Messages = new List<string> { $"Driver with ID {id} not found for editing." } });
                }

                return View(driver);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Messages = new List<string> { $"Error retrieving driver for edit (ID {id}).", ex.Message } });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, DriverDto driverDto)
        {
            if (id != driverDto.DriverId)
            {
                return View("Error", new ErrorViewModel { Messages = new List<string> { "Driver ID mismatch during update." } });
            }

            if (!ModelState.IsValid)
            {
                return View("Edit", driverDto);
            }

            try
            {
                var response = await _driverService.UpdateDriver(driverDto);
                if (response.Status == ServiceResponse.ServiceStatus.Updated)
                {
                    TempData["SuccessMessage"] = "Driver updated successfully!";
                    return RedirectToAction("Details", new { id = driverDto.DriverId });
                }

                return View("Error", new ErrorViewModel { Messages = response.Messages });
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Messages = new List<string> { $"Error updating driver ID {id}.", ex.Message } });
            }
        }

        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                var driver = await _driverService.FindDriver(id);
                return driver == null
                    ? View("Error", new ErrorViewModel { Messages = new List<string> { $"Driver with ID {id} not found for deletion confirmation." } })
                    : View(driver);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Messages = new List<string> { $"Error preparing to delete driver ID {id}.", ex.Message } });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _driverService.DeleteDriver(id);
                if (response.Status == ServiceResponse.ServiceStatus.Deleted)
                {
                    TempData["SuccessMessage"] = "Driver deleted successfully!";
                    return RedirectToAction("List");
                }

                return View("Error", new ErrorViewModel { Messages = response.Messages });
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Messages = new List<string> { $"Error deleting driver ID {id}.", ex.Message } });
            }
        }
    }
}
