using CustomerOrder.Data;
using CustomerOrder.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrder.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerOrderDbContext _dbContext;

        public CustomerRepository(CustomerOrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Customer> CreateAsync(Customer customer)
        {
            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync();

            return customer;
        }

        public async Task<List<Customer>> GetAllAsync() => await _dbContext.Customers.ToListAsync();
    }
}
