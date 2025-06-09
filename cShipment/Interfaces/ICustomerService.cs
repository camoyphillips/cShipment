using cShipment.Models;

namespace cShipment.Interfaces
{
    public interface ICustomerService
    {
        IEnumerable<CustomerDto> ListCustomers();

        Task<CustomerDto?> FindCustomer(int id);

        Task<ServiceResponse> AddCustomer(CustomerDto customerDto);

        Task<ServiceResponse> UpdateCustomer(CustomerDto customerDto);

        Task<ServiceResponse> DeleteCustomer(int id);
    }
}
