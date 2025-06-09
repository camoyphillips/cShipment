using cShipment.Interfaces;
using cShipment.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic; // Added for ListCustomers return type
using Microsoft.AspNetCore.Http; // Added for StatusCodes

namespace cShipment.Controllers
{
    /// <summary>
    /// API controller for customer-related endpoints.
    /// </summary>
    [Route("api/[controller]")] // Consistent Route: /api/Customer
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        /// <summary>
        /// Constructor with dependency injection for ICustomerService.
        /// </summary>
        /// <param name="customerService">Injected customer service.</param>
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Gets a list of all customers from the system.
        /// </summary>
        /// <returns>A list of CustomerDto records.</returns>
        /// <response code="200">Returns a list of CustomerDto records.</response>
        /// <response code="500">If an unhandled error occurs on the server.</response>
        [HttpGet("List")] // Route: GET /api/Customer/List
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<CustomerDto>> ListCustomers() // Changed return type to ActionResult
        {
            try
            {
                return Ok(_customerService.ListCustomers());
            }
            catch (System.Exception ex)
            {
                // Log the exception ex
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving customers: {ex.Message}");
            }
        }

        // You might want to add other CRUD operations (GetById, Create, Update, Delete) for customers here.
        // For example:

        /*
        /// <summary>
        /// Gets a customer by their ID.
        /// </summary>
        /// <param name="id">The ID of the customer to retrieve.</param>
        /// <returns>A CustomerDto record or NotFound.</returns>
        /// <response code="200">Returns the requested customer.</response>
        /// <response code="404">If the customer with the given ID is not found.</response>
        /// <response code="500">If an unhandled error occurs on the server.</response>
        [HttpGet("{id}")] // Route: GET /api/Customer/{id}
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CustomerDto> GetCustomer(int id)
        {
            try
            {
                var customer = _customerService.GetCustomerById(id); // Assuming your service has this method
                if (customer == null)
                {
                    return NotFound($"Customer with ID {id} not found.");
                }
                return Ok(customer);
            }
            catch (System.Exception ex)
            {
                // Log the exception ex
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving customer with ID {id}: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="customerDto">The customer data to create.</param>
        /// <returns>The newly created CustomerDto.</returns>
        /// <response code="201">Returns the newly created customer.</response>
        /// <response code="400">If the customer data is invalid.</response>
        /// <response code="500">If an unhandled error occurs on the server.</response>
        [HttpPost("Create")] // Route: POST /api/Customer/Create
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CustomerDto> CreateCustomer([FromBody] CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdCustomer = _customerService.CreateCustomer(customerDto); // Assuming your service has this method
                return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.CustomerId }, createdCustomer);
            }
            catch (System.Exception ex)
            {
                // Log the exception ex
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating customer: {ex.Message}");
            }
        }
        */
    }
}