namespace EduQuest.Features.Reviews
{
    public class ReviewRequestDto
    {
        public int CourseId { get; set; }

        public int Rating { get; set; }

        public string ReviewText { get; set; }

        public int ReviewedById { get; set; }

    }
}