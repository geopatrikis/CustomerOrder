using CustomerOrder.Exceptions;
using CustomerOrder.Models;
using CustomerOrder.Repositories;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrder.Services
{
    public class OrderService
    {
        private readonly IValidator<Order> _orderValidator;
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        public OrderService(IOrderRepository rep, IValidator<Order> val, ICustomerRepository repCust) {
            _orderRepository = rep;
            _orderValidator = val;
            _customerRepository = repCust;
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
                return createdOrder;
            }
            catch (DbUpdateException ex) when (ForeignKeyException(ex))
            {
                throw new CustomerNotFoundException("Customer not found and order creation failed.", ex);
            }

        }

        public async Task<List<Order>> GetAllCustomerOrders(int customerId)
        {
            return await _orderRepository.GetAllCustomerOrdersAsync(customerId);
        }

        public async Task<Order> CancelOrder(int orderId)
        {
            
            var orderToUpdate = await _orderRepository.GetOrderAsync(orderId);

            if (orderToUpdate == null)
                throw new OrderNotFoundException("Order with id {orderId} was not found");

            orderToUpdate.Cancelled = true; 

            await _orderRepository.UpdateAsync(orderToUpdate); 

            return orderToUpdate;
        }

        private bool ForeignKeyException(DbUpdateException ex)
        {
            return ex.InnerException is SqlException sqlEx && sqlEx.Number == 547;
        }

        public async Task<List<Order>> GetCancelledCustomerOrders(int customerId)
        {
            return await _orderRepository.GetCancelledCustomerOrdersAsync(customerId);
        }

        public async Task<List<Order>> GetOrdersByTimeWindowAsync(DateTime startTime, DateTime endTime)
        {
            return await _orderRepository.GetOrdersByTimeWindowAsync(startTime,endTime);
        }
    }
}
