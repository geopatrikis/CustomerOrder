using CustomerOrder.Models;

namespace CustomerOrder.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> CreateAsync(Customer customer);
        Task<List<Customer>> GetAllAsync();
    }
}