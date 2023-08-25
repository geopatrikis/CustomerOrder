using CustomerOrder.Data;
using CustomerOrder.Models;
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

        public async Task<Customer?> GetCustomerAsync(int id)
        {
            return await _dbContext.Customers
                .SingleOrDefaultAsync(customer => customer.Id == id);
        }

        public async Task<List<Customer>> GetCustomersByEmailAsync(string email)
        {
            return await _dbContext.Customers
                    .Where(customer => customer.Email.Contains(email))
                    .ToListAsync();
        }

        public async Task<Customer> UpdateAsync(Customer existingCustomer)
        {
            _dbContext.Entry(existingCustomer).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return existingCustomer;
        }
    }
}
