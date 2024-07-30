using EduQuest.Commons;

namespace EduQuest.Entities
{
    public class Question : BaseEntity
    {
        public int ContentId { get; set; }

        public string QuestionText { get; set; }

        public int PostedById { get; set; }

        public DateTime PostedOn { get; set; } = DateTime.Now;

        public int Upvotes { get; set; } = 0;

        public Content Content { get; set; }

        public User PostedBy { get; set; }

        public ICollection<Answer> Answers { get; set; }

    }
}
