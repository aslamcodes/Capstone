using EduQuest.Features.Users;

namespace EduQuest.Features.Questions
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public int ContentId { get; set; }

        public string QuestionText { get; set; }

        public int PostedById { get; set; }

        public UserProfileDto PostedBy { get; set; }

        public DateTime PostedOn { get; set; } = DateTime.Now;

        public int Upvotes { get; set; }

    }
}