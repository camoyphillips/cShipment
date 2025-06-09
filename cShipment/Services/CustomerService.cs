using cShipment.Data;
using cShipment.Interfaces;
using cShipment.Models;
using cShipment.Models.Dtos;

namespace cShipment.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CustomerDto> ListCustomers()
        {
            return _context.Customers.Select(c => new CustomerDto
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                CustomerEmail = c.CustomerEmail
            }).ToList();
        }

        public async Task<CustomerDto?> FindCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            return customer == null ? null : new CustomerDto
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
                CustomerEmail = customer.CustomerEmail
            };
        }

        public async Task<ServiceResponse> AddCustomer(CustomerDto dto)
        {
            var customer = new Customer
            {
                CustomerName = dto.CustomerName,
                CustomerEmail = dto.CustomerEmail
            };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return new ServiceResponse { Status = ServiceResponse.ServiceStatus.Created, CreatedId = customer.CustomerId };
        }

        public async Task<ServiceResponse> UpdateCustomer(CustomerDto dto)
        {
            var customer = await _context.Customers.FindAsync(dto.CustomerId);
            if (customer == null) return new ServiceResponse { Status = ServiceResponse.ServiceStatus.NotFound };
            customer.CustomerName = dto.CustomerName;
            customer.CustomerEmail = dto.CustomerEmail;
            await _context.SaveChangesAsync();
            return new ServiceResponse { Status = ServiceResponse.ServiceStatus.Updated };
        }

        public async Task<ServiceResponse> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return new ServiceResponse { Status = ServiceResponse.ServiceStatus.NotFound };
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return new ServiceResponse { Status = ServiceResponse.ServiceStatus.Deleted };
        }
    }
}