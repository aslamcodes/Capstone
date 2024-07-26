using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Payments;

namespace EduQuest.Features.Orders
{
    public class OrderRepo(EduQuestContext context) : BaseRepo<int, Order>(context)
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public int TotalPrice { get; set; }

        public string OrderStatus { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public IEnumerable<Entities.Course> OrderedCourses { get; set; }

        public string PaymentTransactionId { get; set; }
        public PaymentStatusEnum PaymentStatus { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

}
