using CustomerOrder.Models;
using FluentValidation;

namespace CustomerOrder.Services
{

    namespace YourAspNetCoreMVCApp.Services
    {
        public class CustomerValidator : AbstractValidator<Customer>
        {
            public CustomerValidator()
            {
                RuleFor(customer => customer.FirstName).NotEmpty();
                RuleFor(customer => customer.LastName).NotEmpty();
                RuleFor(customer => customer.Email).NotEmpty().EmailAddress();
            }
        }
    }
}
