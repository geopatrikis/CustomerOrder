using CustomerOrder.Models;
using FluentValidation;

namespace CustomerOrder.Services
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(order => order.Description).NotEmpty().MaximumLength(100);
            RuleFor(order => order.Price).GreaterThan(0);
            RuleFor(order => order.CreationDate).NotEmpty();
        }
    }
}
