using CustomerOrder.Models;
using FluentValidation;
using CustomerOrder.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using CustomerOrder.Exceptions;
using CustomerOrder.Cache;
using Microsoft.Extensions.Caching.Memory;

namespace CustomerOrder.Services
{
    public class CustomerService:ICustomerService
    {
        private readonly IMemoryCache _cache;
        private readonly IValidator<Customer> _customerValidator;
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository, IValidator<Customer> customerValidator, IMemoryCache cache)
        {
            _customerRepository = customerRepository;
            _customerValidator = customerValidator;
            _cache = cache;
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            var validationResult = await _customerValidator.ValidateAsync(customer);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                var createdCustomer = await _customerRepository.CreateAsync(customer);
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                   .SetSize(1)
                   .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                _cache.Set($"Customer_{createdCustomer.Id}", createdCustomer,cacheEntryOptions);
                return createdCustomer;
            }
            //I decided to throw exception in order to avoid redundant selects from database to check for existing emails.
            catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
            {
                throw new DuplicateMailException("Email already exists.", ex);
            }

        }
        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            return customers;
        }

        public async Task<List<Customer>> SearchCustomersByEmailAsync(string email)
        {

            var customers = await _customerRepository.GetCustomersByEmailAsync(email);
            return customers;
        }

        public async Task<Customer> UpdateCustomerAsync(int id, Customer updatedCustomer)
        {
            if (updatedCustomer.Id !=0 && updatedCustomer.Id != id)
                throw new CustomerIdMissmatchException("There was an attempt to update the id of the customer. This action is forbidden!");
            try
            {
                var existingCustomer = _cache.TryGetValue($"Customer_{id}", out Customer? cachedCustomer) ? cachedCustomer : null;

                if (existingCustomer == null)
                {
                    // If not found in cache, fetch from the database 
                    existingCustomer = await _customerRepository.GetCustomerAsync(id);

                    if (existingCustomer == null)
                    {
                        throw new CustomerNotFoundException("No customer with id: " + id + " was found. Update failed!");
                    }

                    // Cache the customer from the database
                    //_cache.Set($"Customer_{id}", existingCustomer);
                }
                var validationResult = await _customerValidator.ValidateAsync(updatedCustomer);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
                existingCustomer.Id = id;
                existingCustomer.FirstName = updatedCustomer.FirstName;
                existingCustomer.LastName = updatedCustomer.LastName;
                existingCustomer.Email = updatedCustomer.Email;

                var retcustomer= await _customerRepository.UpdateAsync(existingCustomer);
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                   .SetSize(1)
                   .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                _cache.Set($"Customer_{id}", retcustomer,cacheEntryOptions);
                return retcustomer;
            }
            catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
            {
                throw new DuplicateMailException("Email already exists.", ex);
            }
        }


        private bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            return ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627;
        }
    }
}
