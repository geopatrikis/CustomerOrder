using CustomerOrder.Models;

namespace CustomerOrder.Services
{
    public interface ICustomerService
    {
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task<List<Customer>> GetAllCustomersAsync();
        Task<List<Customer>> SearchCustomersByEmailAsync(string email);
        Task<Customer> UpdateCustomerAsync(int id, Customer updatedCustomer);
    }
}
