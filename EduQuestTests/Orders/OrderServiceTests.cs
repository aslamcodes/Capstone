using AutoMapper;
using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Courses;
using EduQuest.Features.Orders;
using EduQuest.Features.Student;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduQuest.Tests.Features.Orders
{
    [TestFixture]
    public class OrderServiceTests
    {
        private Mock<IRepository<int, Order>> _mockOrderRepo;
        private Mock<ICourseRepo> _mockCourseRepo;
        private Mock<IMapper> _mockMapper;
        private Mock<IStudentService> _mockStudentService;
        private OrderService _orderService;

        [SetUp]
        public void SetUp()
        {
            _mockOrderRepo = new Mock<IRepository<int, Order>>();
            _mockCourseRepo = new Mock<ICourseRepo>();
            _mockMapper = new Mock<IMapper>();
            _mockStudentService = new Mock<IStudentService>();
            _orderService = new OrderService(_mockOrderRepo.Object, _mockCourseRepo.Object, _mockMapper.Object, _mockStudentService.Object);
        }

        [Test]
        public async Task CancelOrder_WhenCalled_ReturnsUpdatedOrder()
        {
            // Arrange
            var orderId = 1;
            var order = new Order { Id = orderId, OrderStatus = OrderStatusEnum.Pending };
            _mockOrderRepo.Setup(r => r.GetByKey(orderId)).ReturnsAsync(order);
            _mockOrderRepo.Setup(r => r.Update(order)).ReturnsAsync(order);
            _mockMapper.Setup(m => m.Map<OrderDto>(order)).Returns(new OrderDto { Id = orderId, OrderStatus = OrderStatusEnum.Cancelled.ToString() });

            // Act
            var result = await _orderService.CancelOrder(orderId);

            // Assert
            Assert.AreEqual(OrderStatusEnum.Cancelled.ToString(), result.OrderStatus);
            _mockOrderRepo.Verify(r => r.Update(order), Times.Once);
        }

        [Test]
        public async Task PendingOrderExists_WhenPendingOrderExists_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var orderedCourseId = 1;
            var orders = new List<Order>
            {
                new Order { UserId = userId, OrderedCourseId = orderedCourseId, OrderStatus = OrderStatusEnum.Pending }
            };
            _mockOrderRepo.Setup(r => r.GetAll()).ReturnsAsync(orders);

            // Act
            var result = await _orderService.PendingOrderExists(userId, orderedCourseId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task CompleteOrder_WhenCalled_ReturnsUpdatedOrder()
        {
            // Arrange
            var orderId = 1;
            var order = new Order { Id = orderId, OrderStatus = OrderStatusEnum.Pending };
            _mockOrderRepo.Setup(r => r.GetByKey(orderId)).ReturnsAsync(order);
            _mockOrderRepo.Setup(r => r.Update(order)).ReturnsAsync(order);
            _mockMapper.Setup(m => m.Map<OrderDto>(order)).Returns(new OrderDto { Id = orderId, OrderStatus = OrderStatusEnum.Completed.ToString() });

            // Act
            var result = await _orderService.CompleteOrder(orderId);

            // Assert
            Assert.AreEqual(OrderStatusEnum.Completed.ToString(), result.OrderStatus);
            _mockOrderRepo.Verify(r => r.Update(order), Times.Once);
        }

        [Test]
        public async Task CreateOrder_WhenCalled_ReturnsNewOrder()
        {
            // Arrange
            var orderRequest = new OrderRequestDto { UserId = 1, OrderedCourse = 1 };
            var course = new Course { Id = 1, Price = 100, EducatorId = 2 };
            var newOrder = new Order { Id = 1, UserId = 1, OrderedCourseId = 1, OrderStatus = OrderStatusEnum.Pending, Price = 100 };

            _mockCourseRepo.Setup(r => r.GetByKey(orderRequest.OrderedCourse)).ReturnsAsync(course);
            _mockOrderRepo.Setup(r => r.Add(It.IsAny<Order>())).ReturnsAsync(newOrder);
            _mockMapper.Setup(m => m.Map<OrderDto>(newOrder)).Returns(new OrderDto { Id = 1 });

            _mockStudentService.Setup(s => s.UserOwnsCourse(orderRequest.UserId, orderRequest.OrderedCourse)).ReturnsAsync(new UserOwnsDto() { UserOwnsCourse = false });

            _mockOrderRepo.Setup(r => r.GetAll()).ReturnsAsync(new List<Order>());

            // Act
            var result = await _orderService.CreateOrder(orderRequest);

            // Assert
            Assert.AreEqual(1, result.Id);
            _mockOrderRepo.Verify(r => r.Add(It.IsAny<Order>()), Times.Once);
        }

        [Test]
        public void CreateOrder_WhenEducatorOwnCourse_ThrowsCannotPlaceOrderException()
        {
            // Arrange
            var orderRequest = new OrderRequestDto { UserId = 1, OrderedCourse = 1 };
            var course = new Course { Id = 1, Price = 100, EducatorId = 1 };

            _mockCourseRepo.Setup(r => r.GetByKey(orderRequest.OrderedCourse)).ReturnsAsync(course);

            // Act & Assert
            var ex = Assert.ThrowsAsync<CannotPlaceOrderException>(async () => await _orderService.CreateOrder(orderRequest));
            Assert.AreEqual("Educator cannot buy his own course", ex.Message);
        }

        [Test]
        public async Task GetOrderById_WhenCalled_ReturnsOrder()
        {
            // Arrange
            var orderId = 1;
            var order = new Order { Id = orderId };
            _mockOrderRepo.Setup(r => r.GetByKey(orderId)).ReturnsAsync(order);
            _mockMapper.Setup(m => m.Map<OrderDto>(order)).Returns(new OrderDto { Id = orderId });

            // Act
            var result = await _orderService.GetOrderById(orderId);

            // Assert
            Assert.AreEqual(orderId, result.Id);
        }

        [Test]
        public async Task GetOrdersForUser_WhenCalled_ReturnsUserOrders()
        {
            // Arrange
            var userId = 1;
            var orders = new List<Order>
            {
                new Order { Id = 1, UserId = userId },
                new Order { Id = 2, UserId = userId }
            };
            var orderDtos = new List<OrderDto>
            {
                new OrderDto { Id = 1 },
                new OrderDto { Id = 2 }
            };
            _mockOrderRepo.Setup(r => r.GetAll()).ReturnsAsync(orders);
            _mockMapper.Setup(m => m.Map<List<OrderDto>>(It.IsAny<List<Order>>())).Returns(orderDtos);

            // Act
            var result = await _orderService.GetOrdersForUser(userId);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual(2, result[1].Id);
        }
    }
}
