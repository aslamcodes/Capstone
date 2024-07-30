using EduQuest.Commons;
using EduQuest.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduQuest.Features.Answers
{
    public class AnswerRepo(EduQuestContext context) : BaseRepo<int, Answer>(context), IAnswerRepo
    {
        public async Task<List<Answer>> GetAnswersByQuestion(int questionId)
        {
            var answers = await context.Answers
                                       .Include(a => a.AnsweredBy)
                                       .Where(answer => answer.QuestionId == questionId)
                                       .AsNoTracking()
                                       .ToListAsync();

            return answers;
        }
    }
}
