using CustomerOrder.Models;
using FluentValidation;
using CustomerOrder.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using CustomerOrder.Exceptions;

namespace CustomerOrder.Services
{
    public class CustomerService
    {
        private readonly IValidator<Customer> _customerValidator;
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository, IValidator<Customer> customerValidator)
        {
            _customerRepository = customerRepository;
            _customerValidator = customerValidator;
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
                return createdCustomer;
            }
            //I decided to throw exception in order to avoid redundant selects from database to check for existing emails.
            catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
            {
                throw new DuplicateMailException("Email already exists.", ex);
            }

        }

        public async Task<List<Customer>> GetAllCustomersAsync() => await _customerRepository.GetAllAsync();

        public async Task<List<Customer>> SearchCustomersByEmailAsync(string email)
        {
            return await _customerRepository.GetCustomersByEmailAsync(email);
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

                return await _customerRepository.UpdateAsync(existingCustomer);
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
