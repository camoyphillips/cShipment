using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using cShipment.Models;
using Microsoft.AspNetCore.Authorization; // Added for potential future authorization

namespace cShipment.Controllers
{
    /// <summary>
    /// Default MVC controller for homepage and error handling.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Displays the landing page for the cShipment web app.
        /// </summary>
        /// <returns>The Index view.</returns>
        public IActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Displays the privacy policy page.
        /// </summary>
        /// <returns>The Privacy view.</returns>

        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Returns the error view with diagnostic information.
        /// </summary>
        /// <returns>The Error view with error details.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}