using EduQuest.Commons;
using EduQuest.Features.Auth.Exceptions;
using EduQuest.Features.Orders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EduQuest.Tests.Features.Orders
{
    [TestFixture]
    public class OrderControllerTests
    {
        private Mock<IOrderService> _mockOrderService;
        private Mock<IControllerValidator> _mockValidator;
        private OrderController _orderController;

        [SetUp]
        public void SetUp()
        {
            _mockOrderService = new Mock<IOrderService>();
            _mockValidator = new Mock<IControllerValidator>();
            _orderController = new OrderController(_mockOrderService.Object, _mockValidator.Object);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.NameIdentifier, "1")
            };
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuthenticationType"));
            _orderController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        [Test]
        public async Task GetOrderDetails_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            var orderId = 1;
            var orderDto = new OrderDto { Id = orderId };
            _mockValidator.Setup(v => v.ValidateUserPrivilageForOrder(It.IsAny<IEnumerable<Claim>>(), orderId)).Returns(Task.CompletedTask);
            _mockOrderService.Setup(s => s.GetOrderById(orderId)).ReturnsAsync(orderDto);

            // Act
            var result = await _orderController.GetOrderDetails(orderId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(orderDto, okResult.Value);
        }

        [Test]
        public async Task GetOrderDetails_WhenUnauthorized_ThrowsUnAuthorisedUserExeception()
        {
            // Arrange
            var orderId = 1;
            _mockValidator.Setup(v => v.ValidateUserPrivilageForOrder(It.IsAny<IEnumerable<Claim>>(), orderId)).Throws<UnAuthorisedUserExeception>();

            // Act
            var result = await _orderController.GetOrderDetails(orderId);

            // Assert
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result.Result);
        }

        [Test]
        public async Task PlaceOrder_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            var orderRequest = new OrderRequestDto { UserId = 1 };
            var orderDto = new OrderDto { Id = 1 };
            _mockValidator.Setup(v => v.ValidateUserPrivilageForUserId(It.IsAny<IEnumerable<Claim>>(), orderRequest.UserId)).Returns(Task.CompletedTask);
            _mockOrderService.Setup(s => s.CreateOrder(orderRequest)).ReturnsAsync(orderDto);

            // Act
            var result = await _orderController.PlaceOrder(orderRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(orderDto, okResult.Value);
        }

        [Test]
        public async Task PlaceOrder_WhenUnauthorized_ThrowsUnAuthorisedUserExeception()
        {
            // Arrange
            var orderRequest = new OrderRequestDto { UserId = 1 };
            _mockValidator.Setup(v => v.ValidateUserPrivilageForUserId(It.IsAny<IEnumerable<Claim>>(), orderRequest.UserId)).Throws<UnAuthorisedUserExeception>();

            // Act
            var result = await _orderController.PlaceOrder(orderRequest);

            // Assert
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result.Result);
        }

        [Test]
        public async Task CancelOrder_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            var orderId = 1;
            var orderDto = new OrderDto { Id = orderId };
            _mockValidator.Setup(v => v.ValidateUserPrivilageForOrder(It.IsAny<IEnumerable<Claim>>(), orderId)).Returns(Task.CompletedTask);
            _mockOrderService.Setup(s => s.CancelOrder(orderId)).ReturnsAsync(orderDto);

            // Act
            var result = await _orderController.CancelOrder(orderId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(orderDto, okResult.Value);
        }

        [Test]
        public async Task CancelOrder_WhenUnauthorized_ThrowsUnAuthorisedUserExeception()
        {
            // Arrange
            var orderId = 1;
            _mockValidator.Setup(v => v.ValidateUserPrivilageForOrder(It.IsAny<IEnumerable<Claim>>(), orderId)).Throws<UnAuthorisedUserExeception>();

            // Act
            var result = await _orderController.CancelOrder(orderId);

            // Assert
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result.Result);
        }

        [Test]
        public async Task GetUserOrders_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            var userId = 1;
            var orderDtoList = new List<OrderDto> { new OrderDto { Id = 1 }, new OrderDto { Id = 2 } };
            _mockValidator.Setup(v => v.GetUserIdFromClaims(It.IsAny<IEnumerable<Claim>>())).Returns(userId);
            _mockOrderService.Setup(s => s.GetOrdersForUser(userId)).ReturnsAsync(orderDtoList);

            // Act
            var result = await _orderController.GetUserOrders();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(orderDtoList, okResult.Value);
        }
    }
}
