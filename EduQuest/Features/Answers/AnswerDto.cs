using EduQuest.Features.Questions;

namespace EduQuest.Features.Answers
{
    public class AnswerDto
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }

        public string AnswerText { get; set; }

        public int AnsweredById { get; set; }

        public DateTime AnsweredOn { get; set; } = DateTime.Now;

        public UserProfileDto AnsweredBy { get; set; }
    }
}