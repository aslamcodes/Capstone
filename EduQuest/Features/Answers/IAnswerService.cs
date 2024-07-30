using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.Answers
{
    public interface IAnswerService : IBaseService<Answer, AnswerDto>
    {
        Task<List<AnswerDto>> GetAnswersForQuestion(int questionId);
    }
}