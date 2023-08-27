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
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
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
        public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
        {
            try
            {
                var createdCustomer = await _customerService.CreateCustomerAsync(customer);
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
                var ret_customer=await _customerService.UpdateCustomerAsync(id, updatedCustomer);
                return Ok(ret_customer);
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
