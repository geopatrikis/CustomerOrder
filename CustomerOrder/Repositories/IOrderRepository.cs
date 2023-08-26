using CustomerOrder.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrder.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<List<Order>> GetAllCustomerOrdersAsync(int customerId);
        Task<List<Order>> GetCancelledCustomerOrdersAsync(int customerId);
        Task<Order?> GetOrderAsync(int orderId);
        Task<List<Order>> GetOrdersByTimeWindowAsync(DateTime startTime, DateTime endTime);
        Task UpdateAsync(Order orderToUpdate);
    }

}
