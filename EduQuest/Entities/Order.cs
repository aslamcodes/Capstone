using EduQuest.Commons;
using EduQuest.Features.Orders;

namespace EduQuest.Entities
{
    public class Order : BaseEntity
    {
        public int UserId { get; set; }

        public float Price { get; set; }

        public OrderStatusEnum OrderStatus { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int OrderedCourseId { get; set; }
        public Course OrderedCourse { get; set; }

        public Payment Payment { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public float DiscountAmount { get; set; }
        public DateTime? CompletedAt { get; set; }

        public User OrderedUser { get; set; }
    }
}
