namespace EduQuest.Features.Answers
{
    public class AnswerRequestDto
    {
        public int QuestionId { get; set; }

        public string AnswerText { get; set; }

        public int AnsweredById { get; set; }

    }
}