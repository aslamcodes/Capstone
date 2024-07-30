using AutoMapper;
using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.Questions
{
    public class QuestionService(IQuestionRepo repo, IMapper mapper) : BaseService<Question, QuestionDto>(repo, mapper), IQuestionService
    {
        public async Task<List<QuestionDto>> GetQuestionsForContent(int contentId)
        {
            var questions = await repo.GetQuestionsByContent(contentId);

            return mapper.Map<List<QuestionDto>>(questions);
        }
    }
}
