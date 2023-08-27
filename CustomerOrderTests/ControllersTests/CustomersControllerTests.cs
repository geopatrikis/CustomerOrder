using CustomerOrder.Controllers;
using CustomerOrder.Exceptions;
using CustomerOrder.Models;
using CustomerOrder.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
namespace CustomerOrderTests.ControllersTests
{
    public class CustomersControllerTests
    {
        [Fact]
        public async Task GetAllCustomers_ReturnsOk()
        {
            var mockCustomerService = new Mock<ICustomerService>();
            var controller = new CustomersController(mockCustomerService.Object);

            var customers = new List<Customer>
        {
            new Customer { Id = 1, FirstName = "Giorgos", LastName = "Pat", Email = "gpatpat@example.com" },
            new Customer { Id = 2, FirstName = "George", LastName = "Pat", Email = "georPatPat@example.com" }
        };

            mockCustomerService.Setup(service =>
                service.GetAllCustomersAsync())
                .ReturnsAsync(customers);

            var result = await controller.GetAllCustomers();

            result.Should().BeOfType<ActionResult<List<Customer>>>();
            var actionResult = result as ActionResult<List<Customer>>;
            actionResult.Should().NotBeNull();
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var okResult = actionResult.Result as OkObjectResult;
        }

        [Fact]
        public async Task SearchCustomersByEmail_CustomersExist_ReturnsOk()
        {
            var mockCustomerService = new Mock<ICustomerService>();
            var controller = new CustomersController(mockCustomerService.Object);

            var email = "patpat@example.com";
            var customers = new List<Customer>
        {
            new Customer { Id = 1, FirstName = "George", LastName = "Patpat", Email = "patpatpat@example.com" }
        };

            mockCustomerService.Setup(service =>
                service.SearchCustomersByEmailAsync(email))
                .ReturnsAsync(customers);

            var result = await controller.SearchCustomersByEmail(email);

            result.Should().BeOfType<ActionResult<List<Customer>>>();
            var actionResult = result as ActionResult<List<Customer>>;
            actionResult.Should().NotBeNull();
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var okResult = actionResult.Result as OkObjectResult;
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task CreateCustomer_ValidData_ReturnsOk()
        {
            var mockCustomerService = new Mock<ICustomerService>();
            var controller = new CustomersController(mockCustomerService.Object);

            var newCustomer = new Customer
            {
                FirstName = "George",
                LastName = "Pat",
                Email = "patpat@example.com"
            };

            var createdCustomer = new Customer
            {
                Id = 1,
                FirstName = "George",
                LastName = "Pat",
                Email = "patpat@example.com"
            };

            mockCustomerService.Setup(service =>
                service.CreateCustomerAsync(newCustomer))
                .ReturnsAsync(createdCustomer);

            var result = await controller.CreateCustomer(newCustomer);

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task UpdateCustomer_ValidData_ReturnsOk()
        {
            var mockCustomerService = new Mock<ICustomerService>();
            var controller = new CustomersController(mockCustomerService.Object);

            var id = 1;
            var updatedCustomer = new Customer
            {
                FirstName = "George",
                LastName = "Pat",
                Email = "patpat@example.com"
            };

            var ret_customer = new Customer
            {
                Id = 1,
                FirstName = "George",
                LastName = "Pat",
                Email = "patpat@example.com"
            };

            mockCustomerService.Setup(service =>
                service.UpdateCustomerAsync(id, updatedCustomer))
                .ReturnsAsync(ret_customer);

            var result = await controller.UpdateCustomer(id, updatedCustomer);

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}
