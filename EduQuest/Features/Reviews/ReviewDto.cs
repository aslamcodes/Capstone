using EduQuest.Features.Questions;
using EduQuest.Features.Users;

namespace EduQuest.Features.Reviews
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }

        public int Rating { get; set; }

        public string ReviewText { get; set; }

        public int ReviewedById { get; set; }

        public UserProfileDto ReviewedBy { get; set; }
    }
}