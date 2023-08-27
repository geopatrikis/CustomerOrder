using CustomerOrder.Cache;
using CustomerOrder.Exceptions;
using CustomerOrder.Models;
using CustomerOrder.Repositories;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CustomerOrder.Services
{
    public class OrderService
    {
        private readonly IValidator<Order> _orderValidator;
        private readonly IOrderRepository _orderRepository;
        private readonly MyMemoryCache _cache;
        public OrderService(IOrderRepository rep, IValidator<Order> val, MyMemoryCache cache) {
            _orderRepository = rep;
            _orderValidator = val;
            _cache = cache;
        }

        public async Task<Order> CreateOrderForCustomerAsync(int customerId,Order order)
        {
            if (order.CreationDate == default(DateTime))
            {
                order.CreationDate = DateTime.UtcNow;
            }

            order.CustomerId = customerId;

            var validationResult = await _orderValidator.ValidateAsync(order);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            try
            {
                var createdOrder = await _orderRepository.CreateOrderAsync(order);
                _cache.Remove($"CustomerOrders_{customerId}");
                return createdOrder;
            }
            catch (DbUpdateException ex) when (ForeignKeyException(ex))
            {
                throw new CustomerNotFoundException("Customer not found and order creation failed.", ex);
            }

        }
        public async Task<List<Order>?> GetAllCustomerOrders(int customerId)
        {
            var cacheKey = $"CustomerOrders_{customerId}";

            if (_cache.TryGetValue(cacheKey, out List<Order>? cachedOrders))
            {
                if (cachedOrders != null)
                {
                    return cachedOrders;
                }
            }
            var customerOrders = await _orderRepository.GetAllCustomerOrdersAsync(customerId);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSize(customerOrders.Count)  
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)); 

            if( customerOrders != null && customerOrders.Count>0) 
                _cache.Set(cacheKey, customerOrders, cacheEntryOptions);
            return customerOrders;
            
        }

        public async Task<Order> CancelOrder(int orderId)
        {
            
            var orderToUpdate = await _orderRepository.GetOrderAsync(orderId);

            if (orderToUpdate == null)
                throw new OrderNotFoundException("Order with id "+ orderId+" was not found");

            orderToUpdate.Cancelled = true; 

            await _orderRepository.UpdateAsync(orderToUpdate);
            _cache.Remove($"CustomerCancelledOrders_{orderToUpdate.CustomerId}");
            return orderToUpdate;
        }


        public async Task<List<Order>> GetCancelledCustomerOrders(int customerId)
        {
            var cacheKey = $"CustomerCancelledOrders_{customerId}";

            if (_cache.TryGetValue(cacheKey, out List<Order>? cachedOrders))
            {
                if(cachedOrders != null)
                    return cachedOrders;
            }
            
            var cancelledOrders = await _orderRepository.GetCancelledCustomerOrdersAsync(customerId);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                   .SetSize(cancelledOrders.Count)
                   .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            _cache.Set(cacheKey, cancelledOrders, cacheEntryOptions);
            return cancelledOrders;
           
        }


        public async Task<List<Order>> GetOrdersByTimeWindowAsync(DateTime startTime, DateTime endTime)
        {
            return await _orderRepository.GetOrdersByTimeWindowAsync(startTime,endTime);
        }


        private bool ForeignKeyException(DbUpdateException ex)
        {
            return ex.InnerException is SqlException sqlEx && sqlEx.Number == 547;
        }
    }
}
