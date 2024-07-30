using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.Answers
{
    public interface IAnswerRepo : IRepository<int, Answer>
    {
        public Task<List<Answer>> GetAnswersByQuestion(int questionId);
    }
}
