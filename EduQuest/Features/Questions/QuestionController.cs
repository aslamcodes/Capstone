using AutoMapper;
using EduQuest.Commons;
using EduQuest.Features.Auth.Exceptions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest.Features.Questions
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController(IQuestionService questionService, IMapper mapper, IControllerValidator validator)
        : ControllerBase
    {
        [HttpGet("For-Content")]
        public async Task<ActionResult<QuestionDto>> GetQuestionsForContent([FromQuery] int contentId)
        {
            try
            {
                await validator.ValidateUserPrivilegeForContent(User.Claims, contentId);
                var questions = await questionService.GetQuestionsForContent(contentId);
                return Ok(questions);
            }
            catch (UnAuthorisedUserExeception)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized,
                    "Unauthorized access to the resource"));
            }
            catch (EntityNotFoundException)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, "Resource not found"));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult<QuestionDto>> PostQuestion([FromBody] QuestionRequestDto questionDto)
        {
            try
            {
                await validator.ValidateUserPrivilageForUserId(User.Claims, questionDto.PostedById);


                var question = await questionService.Add(new QuestionDto
                {
                    ContentId = questionDto.ContentId,
                    PostedById = questionDto.PostedById,
                    PostedOn = DateTime.Now,
                    QuestionText = questionDto.QuestionText,
                });
                return Ok(question);
            }
            catch (UnAuthorisedUserExeception)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized,
                    "Unauthorized access to the resource"));
            }
            catch (EntityNotFoundException)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, "Resource not found"));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteQuestion([FromQuery] int id)
        {
            try
            {
                var question = await questionService.GetById(id);
                await validator.ValidateUserPrivilageForUserId(User.Claims, question.PostedById);
                await questionService.DeleteById(id);
                return Ok(question);
            }
            catch (UnAuthorisedUserExeception)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized,
                    "Unauthorized access to the resource"));
            }
            catch (EntityNotFoundException)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, "Resource not found"));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}