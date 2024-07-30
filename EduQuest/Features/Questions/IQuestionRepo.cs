using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.Questions
{
    public interface IQuestionRepo : IRepository<int, Question>
    {
        Task<List<Question>> GetQuestionsByContent(int contentId);
    }
}