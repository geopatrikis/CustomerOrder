using CustomerOrder.Models;

namespace CustomerOrder.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> CreateAsync(Customer customer);
        Task<List<Customer>> GetAllAsync();
        Task<Customer?> GetCustomerAsync(int id);
        Task<List<Customer>> GetCustomersByEmailAsync(string email);
        Task<Customer> UpdateAsync(Customer existingCustomer);
    }
}