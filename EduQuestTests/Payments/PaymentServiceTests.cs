using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Payments;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using EduQuest.Features.Orders;

namespace EduQuestTests.Payments
{
    [TestFixture]
    public class PaymentServiceTests
    {
        private Mock<IRepository<int, Payment>> _mockPaymentRepo;
        private Mock<IRepository<int, Order>> _mockOrderRepo;
        private Mock<IRepository<int, User>> _mockUserRepo;
        private PaymentService _paymentService;

        [SetUp]
        public void SetUp()
        {
            _mockPaymentRepo = new Mock<IRepository<int, Payment>>();
            _mockOrderRepo = new Mock<IRepository<int, Order>>();
            _mockUserRepo = new Mock<IRepository<int, User>>();
            _paymentService = new PaymentService(_mockPaymentRepo.Object, _mockOrderRepo.Object, _mockUserRepo.Object);
        }

        [Test]
        public async Task MakePaymentForOrder_WhenOrderIsPending_ReturnsPayment()
        {
            // Arrange
            var orderId = 1;
            var order = new Order { Id = orderId, Price = 100, OrderStatus = OrderStatusEnum.Pending };
            _mockOrderRepo.Setup(repo => repo.GetByKey(orderId)).ReturnsAsync(order);

            // Act
            var result = await _paymentService.MakePaymentForOrder(orderId);

            // Assert
            _mockPaymentRepo.Verify(repo => repo.Add(It.IsAny<Payment>()), Times.Once);
            Assert.AreEqual(orderId, result.OrderId);
            Assert.AreEqual(100, result.Amount);
            Assert.AreEqual(PaymentStatusEnum.Paid, result.PaymentStatus);
            Assert.AreEqual(order.Price, result.Amount);
            Assert.IsNotNull(result.PaymentTransactionId);
            Assert.AreEqual(orderId, result.OrderId);
        }

        [Test]
        public void MakePaymentForOrder_WhenOrderIsNotPending_ThrowsCannotMakePaymentException()
        {
            // Arrange
            var orderId = 1;
            var order = new Order { Id = orderId, Price = 100, OrderStatus = OrderStatusEnum.Completed };
            _mockOrderRepo.Setup(repo => repo.GetByKey(orderId)).ReturnsAsync(order);

            // Act & Assert
            var ex = Assert.ThrowsAsync<CannotMakePaymentException>(() => _paymentService.MakePaymentForOrder(orderId));
            Assert.AreEqual("Order is not pending", ex.Message);
            _mockPaymentRepo.Verify(repo => repo.Add(It.IsAny<Payment>()), Times.Never);
        }
    }
}
