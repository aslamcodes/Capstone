using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using EduQuest.Features.Payments;
using EduQuest.Features.Auth.Exceptions;
using EduQuest.Features.Courses;
using EduQuest.Features.Orders;
using EduQuest.Commons;
using EduQuest.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using EduQuest.Features.Courses.Dto;

namespace EduQuestTests.Payments
{
    [TestFixture]
    public class PaymentControllerTests
    {
        private Mock<IPaymentService> _mockPaymentService;
        private Mock<IOrderService> _mockOrderService;
        private Mock<ICourseService> _mockCourseService;
        private Mock<IControllerValidator> _mockValidator;
        private PaymentsController _controller;

        [SetUp]
        public void Setup()
        {
            _mockPaymentService = new Mock<IPaymentService>();
            _mockOrderService = new Mock<IOrderService>();
            _mockCourseService = new Mock<ICourseService>();
            _mockValidator = new Mock<IControllerValidator>();

            _controller = new PaymentsController(_mockPaymentService.Object, _mockOrderService.Object, _mockCourseService.Object, _mockValidator.Object);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.NameIdentifier, "1")
            };
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuthenticationType"));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

        }

        [Test]
        public async Task MakePaymentForOrder_ShouldReturnOkResultWithPayment()
        {
            // Arrange
            int orderId = 1;
            var payment = new Payment { Id = 1, OrderId = orderId, Amount = 100, PaymentStatus = PaymentStatusEnum.Paid};
            var order = new Order { Id = orderId, UserId = 1, OrderedCourseId = 1, OrderStatus = OrderStatusEnum.Completed };

            _mockValidator.Setup(v => v.ValidateUserPrivilageForOrder(It.IsAny<IEnumerable<Claim>>(), orderId)).Returns(Task.CompletedTask);
            _mockPaymentService.Setup(service => service.MakePaymentForOrder(orderId)).ReturnsAsync(payment);
            _mockOrderService.Setup(service => service.CompleteOrder(orderId)).ReturnsAsync(new OrderDto()
            {
                Id = orderId,
                UserId = order.UserId,
                OrderedCourseId = order.OrderedCourseId,
                OrderStatus = OrderStatusEnum.Completed.ToString()
            });
            _mockCourseService.Setup(service => service.EnrollStudentIntoCourse(order.UserId, order.OrderedCourseId)).ReturnsAsync(new CourseDTO()
            {
                Id =  order.OrderedCourseId,
                Name = "Test Course",
                Description = "Test Description",
               
            });

            // Act
            var result = await _controller.MakePaymentForOrder(orderId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(payment, okResult.Value);
        }

        [Test]
        public async Task MakePaymentForOrder_ShouldReturnBadRequestOnCannotMakePaymentException()
        {
            // Arrange
            int orderId = 1;

            _mockValidator.Setup(v => v.ValidateUserPrivilageForOrder(It.IsAny<IEnumerable<Claim>>(), orderId)).Returns(Task.CompletedTask);
            _mockPaymentService.Setup(service => service.MakePaymentForOrder(orderId)).Throws(new CannotMakePaymentException("Cannot make payment"));

            // Act
            var result = await _controller.MakePaymentForOrder(orderId);

            // Assert
            var badRequestResult = result.Result as ObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Cannot make payment", ((ErrorModel)badRequestResult.Value).Message);
        }

        [Test]
        public async Task MakePaymentForOrder_ShouldReturnNotFoundOnEntityNotFoundException()
        {
            // Arrange
            int orderId = 1;

            _mockValidator.Setup(v => v.ValidateUserPrivilageForOrder(It.IsAny<IEnumerable<Claim>>(), orderId)).Returns(Task.CompletedTask);
            _mockPaymentService.Setup(service => service.MakePaymentForOrder(orderId)).Throws(new EntityNotFoundException());

            // Act
            var result = await _controller.MakePaymentForOrder(orderId);

            // Assert
            var notFoundResult = result.Result as ObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task MakePaymentForOrder_ShouldReturnUnauthorizedOnUnAuthorisedUserExeception()
        {
            // Arrange
            int orderId = 1;
         
            _mockValidator.Setup(v => v.ValidateUserPrivilageForOrder(It.IsAny<IEnumerable<Claim>>(), orderId)).Throws(new UnAuthorisedUserExeception());

            // Act
            var result = await _controller.MakePaymentForOrder(orderId);

            // Assert
            var unauthorizedResult = result.Result as ObjectResult;
            Assert.IsNotNull(unauthorizedResult);
            Assert.AreEqual(401, unauthorizedResult.StatusCode);
        }
    }
}
