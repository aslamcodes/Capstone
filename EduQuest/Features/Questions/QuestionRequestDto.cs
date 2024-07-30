namespace EduQuest.Features.Questions
{
    public class QuestionRequestDto
    {
        public int ContentId { get; set; }

        public string QuestionText { get; set; }

        public int PostedById { get; set; }

    }
}