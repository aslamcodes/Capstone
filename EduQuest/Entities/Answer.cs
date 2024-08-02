using System.Diagnostics.CodeAnalysis;
using EduQuest.Commons;

namespace EduQuest.Entities
{
    [ExcludeFromCodeCoverage]
    public class Answer : BaseEntity
    {
        public int QuestionId { get; set; }

        public string AnswerText { get; set; }

        public int AnsweredById { get; set; }

        public DateTime AnsweredOn { get; set; } = DateTime.Now;

        public int Upvotes { get; set; }

        public Question Question { get; set; }

        public User AnsweredBy { get; set; }
    }
}
