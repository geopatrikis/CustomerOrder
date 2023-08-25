using CustomerOrder.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrder.Data
{
    public class CustomerOrderDbContext: DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }

        public CustomerOrderDbContext(DbContextOptions<CustomerOrderDbContext> options) : base(options)
        {

        }
    
    }
}
