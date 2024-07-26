using EduQuest.Commons;
using EduQuest.Features.Payments;

namespace EduQuest.Entities
{
    public class Payment : BaseEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string PaymentTransactionId { get; set; }
        public PaymentStatusEnum PaymentStatus { get; set; }
        public DateTime? ProcessedAt { get; set; }

        public Order Order { get; set; }
        public float Amount { get; set; }
    }
}
