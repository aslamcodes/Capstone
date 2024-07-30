using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest.Features.Answers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController(IAnswerService answerService, IMapper mapper) : Controller
    {
        [HttpGet("For-Question")]
        [Authorize]
        public async Task<ActionResult<AnswerDto>> GetAnswersForQuestion([FromQuery] int questionId)
        {
            try
            {
                //TODO: Assert validity of the user to perform this action
                var answers = await answerService.GetAnswersForQuestion(questionId);

                return Ok(answers);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("For-Question")]
        [Authorize]
        public async Task<ActionResult<AnswerDto>> PostAnswer([FromBody] AnswerRequestDto answerDto)
        {
            try
            {
                //TODO: Assert validity of the user to perform this action  

                var answer = await answerService.Add(mapper.Map<AnswerDto>(answerDto));

                return Ok(answer);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
