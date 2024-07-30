using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.Questions
{
    public interface IQuestionService : IBaseService<Question, QuestionDto>
    {
        Task<List<QuestionDto>> GetQuestionsForContent(int contentId);
    }
}