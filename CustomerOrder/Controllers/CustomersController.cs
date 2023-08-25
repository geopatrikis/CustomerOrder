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

        [HttpPost]
        public IActionResult CreateCustomer([FromBody] Customer customer)
        {
            try
            {
                // Call the service method to create the customer
                var createdCustomer = _customerService.CreateCustomerAsync(customer).Result;

                return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.Id }, createdCustomer);
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

        [HttpGet("{id}", Name = "GetCustomer")]
        public IActionResult GetCustomer(int id)
        {
            // Implement the logic to retrieve a customer by their ID
            // You'll need to call the appropriate method in your service or repository
            // Return the customer or a NotFound response if the customer doesn't exist
            return NotFound();  // Return a 404 Not Found response
        }
        
    }
}
