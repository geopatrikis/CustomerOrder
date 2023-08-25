using CustomerOrder.Exceptions;
using CustomerOrder.Models;
using CustomerOrder.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CustomerOrder.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerService _customerService;
        private readonly OrderService _orderService;
        private readonly IValidator<Order> _orderValidator;

        public CustomersController(CustomerService customerService, OrderService orderService, IValidator<Order> orderValidator)
        {
            _customerService = customerService;
            _orderService = orderService;
            _orderValidator = orderValidator;
        }

        [HttpGet]
        public async Task<ActionResult<List<Customer>>> GetAllCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Customer>>> SearchCustomersByEmail([FromQuery] string email)
        {
            var customers = await _customerService.SearchCustomersByEmailAsync(email);
            if(customers.Count() > 0)
                return Ok(customers);
            else 
                return NoContent();
            
        }

        [HttpPost]
        public IActionResult CreateCustomer([FromBody] Customer customer)
        {
            try
            {
                var createdCustomer = _customerService.CreateCustomerAsync(customer).Result;

                return Ok(createdCustomer);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, Customer updatedCustomer)
        {
            try
            {
                await _customerService.UpdateCustomerAsync(id, updatedCustomer);
                return Ok(updatedCustomer);
            }
            catch (CustomerNotFoundException ex)
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
