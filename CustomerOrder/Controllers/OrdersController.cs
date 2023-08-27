using CustomerOrder.Exceptions;
using CustomerOrder.Models;
using CustomerOrder.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CustomerOrder.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : Controller
    {
        private readonly OrderService _orderService;


        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("customer/{customerId}")]
        public IActionResult CreateOrderForCustomer(int customerId, [FromBody] Order order)
        {
            try
            {
                var createdOrder = _orderService.CreateOrderForCustomerAsync(customerId, order).Result;

                return Ok(createdOrder);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<List<Customer>>> GetCustomerOrders(int customerId)
        {
            var orders = await _orderService.GetAllCustomerOrders(customerId);
            if (orders!=null || orders.Count() > 0)
                return Ok(orders);
            else
                return NoContent();
        }

        [HttpGet("searchbytime")]
        public async Task<IActionResult> SearchOrdersByTimeWindow([FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
        {
            try
            {
                var orders = await _orderService.GetOrdersByTimeWindowAsync(startTime, endTime);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("customer/{customerId}/Cancelled")]
        public async Task<ActionResult<List<Customer>>> GetCancelledCustomerOrders(int customerId)
        {
            var orders = await _orderService.GetCancelledCustomerOrders(customerId);
            if (orders.Count() > 0)
                return Ok(orders);
            else
                return NoContent();
        }

        [HttpPut("{orderId}/cancel")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            try
            {
                var order = await _orderService.CancelOrder(orderId);
                return Ok(order);
            }
            catch (OrderNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
