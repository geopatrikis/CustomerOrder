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
    public class OrdersControllerTests
    {
        [Fact]
        public async Task CreateOrderForCustomer_ValidationFails_ReturnsBadRequest()
        {
            var mockOrderService = new Mock<IOrderService>();
            var controller = new OrdersController(mockOrderService.Object);

            var customerId = 1;
            var order = new Order
            {
                Description = "This sentence contains more than 100 characters..."
            };

            mockOrderService.Setup(service =>
                service.CreateOrderForCustomerAsync(customerId, order))
                .ThrowsAsync(new FluentValidation.ValidationException("Validation failed"));

            var result = await controller.CreateOrderForCustomer(customerId, order);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateOrderForCustomer_OrderCreationSucceeds_ReturnsOk()
        {
            var mockOrderService = new Mock<IOrderService>();
            var controller = new OrdersController(mockOrderService.Object);

            var customerId = 1;
            var order = new Order
            {
                Description = "Valid description",
                Price = 100
            };

            mockOrderService.Setup(service =>
                service.CreateOrderForCustomerAsync(customerId, order))
                .ReturnsAsync(new Order { Id = 123,
                Description="asda"});

            var result = await controller.CreateOrderForCustomer(customerId, order);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task CreateOrderForCustomer_ServiceThrowsException_ReturnsInternalServerError()
        {
            var mockOrderService = new Mock<IOrderService>();
            var controller = new OrdersController(mockOrderService.Object);

            var customerId = 1;
            var order = new Order { 
            Description="Service exception"
            };

            mockOrderService.Setup(service =>
                service.CreateOrderForCustomerAsync(customerId, order))
                .ThrowsAsync(new Exception("Service exception"));

            var result = await controller.CreateOrderForCustomer(customerId, order);

            result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task GetCustomerOrders_OrdersExist_ReturnsOk()
        {
            var mockOrderService = new Mock<IOrderService>();
            var controller = new OrdersController(mockOrderService.Object);

            var customerId = 1;
            var orders = new List<Order>
            {
                new Order { Id = 123 , Description="asda"},
                new Order { Id = 456 , Description="asda"}
            };

            mockOrderService.Setup(service =>
                service.GetAllCustomerOrders(customerId))
                .ReturnsAsync(orders);

            var result = await controller.GetCustomerOrders(customerId);

            result.Should().BeOfType<ActionResult<List<Order>>>();
            var actionResult = result as ActionResult<List<Order>>;
            actionResult.Should().NotBeNull();
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var okResult = actionResult.Result as OkObjectResult;
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task SearchOrdersByTimeWindow_OrdersExist_ReturnsOk()
        {
            var mockOrderService = new Mock<IOrderService>();
            var controller = new OrdersController(mockOrderService.Object);

            var startTime = DateTime.UtcNow.AddHours(-1);
            var endTime = DateTime.UtcNow;

            var orders = new List<Order>
            {
                new Order { Id = 123 , Description="asda"},
                new Order { Id = 456 , Description="asda"}
            };

            mockOrderService.Setup(service =>
                service.GetOrdersByTimeWindowAsync(startTime, endTime))
                .ReturnsAsync(orders);

            var result = await controller.SearchOrdersByTimeWindow(startTime, endTime);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetCancelledCustomerOrders_OrdersExist_ReturnsOk()
        {
            var mockOrderService = new Mock<IOrderService>();
            var controller = new OrdersController(mockOrderService.Object);

            var customerId = 1;
            var orders = new List<Order>
            {
                new Order { Id = 123 , Description="asda", Cancelled=true},
                new Order { Id = 456 , Description="asda", Cancelled= true}
            };

            mockOrderService.Setup(service =>
                service.GetCancelledCustomerOrders(customerId))
                .ReturnsAsync(orders);

            var result = await controller.GetCancelledCustomerOrders(customerId);

            result.Should().BeOfType<ActionResult<List<Order>>>();
            var actionResult = result as ActionResult<List<Order>>;
            actionResult.Should().NotBeNull();
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var okResult = actionResult.Result as OkObjectResult;
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task CancelOrder_OrderCancellationSucceeds_ReturnsOk()
        {
            var mockOrderService = new Mock<IOrderService>();
            var controller = new OrdersController(mockOrderService.Object);

            var orderId = 123;
            var order = new Order { Id = orderId, Cancelled = false ,Description="asda"};

            mockOrderService.Setup(service =>
                service.CancelOrder(orderId))
                .ReturnsAsync(order);

            var result = await controller.CancelOrder(orderId);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task CancelOrder_OrderCancellationFails_NoId()
        {
            var mockOrderService = new Mock<IOrderService>();
            var controller = new OrdersController(mockOrderService.Object);

            var orderId = 12345;
            var order = new Order { Id = orderId, Cancelled = false, Description = "asda" };

            mockOrderService.Setup(service =>
                service.CancelOrder(orderId))
                .ThrowsAsync(new OrderNotFoundException("Validation failed"));

            var result = await controller.CancelOrder(orderId);

            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}

