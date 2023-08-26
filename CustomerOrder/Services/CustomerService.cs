using CustomerOrder.Models;
using FluentValidation;
using CustomerOrder.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using CustomerOrder.Exceptions;
using Microsoft.Extensions.Caching.Memory;

namespace CustomerOrder.Services
{
    public class CustomerService
    {
        private readonly IValidator<Customer> _customerValidator;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMemoryCache _cache; 
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

            // Perform additional business logic if needed
            try
            {
                var createdCustomer = await _customerRepository.CreateAsync(customer);
                _cache.Remove("AllCustomers");
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
            var cacheKey = "AllCustomers"; 

            var cachedCustomers = _cache.Get<List<Customer>>(cacheKey);
            if (cachedCustomers != null)
            {
                Console.WriteLine("from cache");
                return cachedCustomers;
            }

            var customers = await _customerRepository.GetAllAsync();

            _cache.Set(cacheKey, customers, TimeSpan.FromMinutes(10));

            return customers;
        }

        public async Task<List<Customer>> SearchCustomersByEmailAsync(string email)
        {
            //This stores in the cache the searched string and not the actual email.
            var cacheKey = $"email_{email}"; 

            var cachedCustomers = _cache.Get<List<Customer>>(cacheKey);
            if (cachedCustomers != null)
            {
                Console.WriteLine("from cache");
                return cachedCustomers;
            }

            var customers = await _customerRepository.GetCustomersByEmailAsync(email);

            _cache.Set(cacheKey, customers, TimeSpan.FromMinutes(10));

            return customers;
        }

        public async Task<Customer> UpdateCustomerAsync(int id, Customer updatedCustomer)
        {
            try
            {
                var existingCustomer = await _customerRepository.GetCustomerAsync(id);
                if (existingCustomer == null)
                {
                    throw new CustomerNotFoundException("No customer with id: " + id + " was found. Update failed!");
                }

                var validationResult = await _customerValidator.ValidateAsync(updatedCustomer);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                existingCustomer.FirstName = updatedCustomer.FirstName;
                existingCustomer.LastName = updatedCustomer.LastName;
                existingCustomer.Email = updatedCustomer.Email;

                var retcustomer= await _customerRepository.UpdateAsync(existingCustomer);
                _cache.Remove("AllCustomers");
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
