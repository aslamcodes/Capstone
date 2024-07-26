
using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.Payments
{
    public class PaymentService(IRepository<int, Payment> paymentRepo, IRepository<int, Order> orderRepo, IRepository<int, Entities.User> userRepo) : IPaymentService
    {
        public async Task<Payment> MakePaymentForOrder(int orderId)
        {
            var order = await orderRepo.GetByKey(orderId);

            if (order.OrderStatus != Orders.OrderStatusEnum.Pending)
            {
                throw new CannotMakePaymentException("Order is not pending");
            }

            var payment = new Payment()
            {
                Amount = order.Price,
                OrderId = orderId,
                PaymentTransactionId = Guid.NewGuid().ToString(),
                PaymentStatus = PaymentStatusEnum.Paid,
                ProcessedAt = DateTime.Now,
            };

            await paymentRepo.Add(payment);

            return payment;


        }
    }
}
