
using EduQuest.Entities;

namespace EduQuest.Features.Payments
{
    public interface IPaymentService
    {
        Task<Payment> MakePaymentForOrder(int orderId);
    }
}