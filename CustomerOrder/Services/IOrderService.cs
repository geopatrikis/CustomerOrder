using CustomerOrder.Models;

namespace CustomerOrder.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderForCustomerAsync(int customerId, Order order);
        Task<List<Order>?> GetAllCustomerOrders(int customerId);
        Task<Order> CancelOrder(int orderId);
        Task<List<Order>> GetCancelledCustomerOrders(int customerId);
        Task<List<Order>> GetOrdersByTimeWindowAsync(DateTime startTime, DateTime endTime);
    }
}
