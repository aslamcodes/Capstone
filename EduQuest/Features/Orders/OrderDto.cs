namespace EduQuest.Features.Orders
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public int TotalPrice { get; set; }

        public string OrderStatus { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int OrderedCourseId { get; set; }

        public DateTime? ProcessedAt { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}