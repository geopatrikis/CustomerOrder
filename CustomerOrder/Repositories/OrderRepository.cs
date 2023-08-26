using CustomerOrder.Data;
using CustomerOrder.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrder.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CustomerOrderDbContext _dbContext;

        public OrderRepository(CustomerOrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Order> CreateOrderAsync(Order order)
        {
                _dbContext.Orders.Add(order);
                await _dbContext.SaveChangesAsync();

                return order;
            
        }

        public async Task<List<Order>> GetAllCustomerOrdersAsync(int customerId)
        {
            return await _dbContext.Orders
                                .Where(order => order.CustomerId == customerId)
                                .ToListAsync();
        }

        public async Task<List<Order>> GetCancelledCustomerOrdersAsync(int customerId)
        {
            return await _dbContext.Orders
                                            .Where(order => order.CustomerId == customerId && order.Cancelled)
                                            .ToListAsync();
        }

        public async Task<Order?> GetOrderAsync(int orderId)
        {
            return await _dbContext.Orders.FindAsync(orderId);
        }

        public async Task<List<Order>> GetOrdersByTimeWindowAsync(DateTime startTime, DateTime endTime)
        {
            return await _dbContext.Orders
                 .Where(order => order.CreationDate >= startTime && order.CreationDate <= endTime)
                    .ToListAsync();
        }

        public async Task UpdateAsync(Order orderToUpdate)
        {
            _dbContext.Orders.Update(orderToUpdate);
            await _dbContext.SaveChangesAsync();
        }
    }
}
