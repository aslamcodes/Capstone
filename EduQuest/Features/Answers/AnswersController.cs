using AutoMapper;
using EduQuest.Commons;
using EduQuest.Features.Auth.Exceptions;
using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest.Features.Answers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController(IAnswerService answerService, IMapper mapper, IControllerValidator validator) : Controller
    {
        [HttpGet("For-Question")]
        [Authorize]
        public async Task<ActionResult<AnswerDto>> GetAnswersForQuestion([FromQuery] int questionId)
        {
            try
            {
                var answers = await answerService.GetAnswersForQuestion(questionId);

                return Ok(answers);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("For-Question")]
        [Authorize]
        public async Task<ActionResult<AnswerDto>> PostAnswer([FromBody] AnswerRequestDto answerDto)
        {
            try
            {
                await validator.ValidateUserPrivilageForUserId(User.Claims, answerDto.AnsweredById);

                var answer = await answerService.Add(mapper.Map<AnswerDto>(answerDto));

                return Ok(answer);
            }
            catch (UnAuthorisedUserExeception)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized,
                    "You are not authorised to perform this action"));
            }
            catch (ReferenceConstraintException)
            {
                return BadRequest(new ErrorModel(StatusCodes.Status400BadRequest,
                    "The question you are trying to answer does not exist"));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
