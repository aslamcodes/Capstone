using AutoMapper;
using EduQuest.Commons;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest.Features.Questions
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController(IQuestionService questionService, IMapper mapper, ControllerValidator validator) : ControllerBase
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
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<QuestionDto>> PostQuestion([FromBody] QuestionRequestDto questionDto)
        {
            try
            {
                if (ControllerValidator.GetUserIdFromClaims(User.Claims) != questionDto.PostedById)
                {
                    return Unauthorized(
                        new ErrorModel(StatusCodes.Status401Unauthorized, "Unauthorized access to the resource")
                        );
                };

                var question = await questionService.Add(new QuestionDto
                {
                    ContentId = questionDto.ContentId,
                    PostedById = questionDto.PostedById,
                    PostedOn = DateTime.Now,
                    QuestionText = questionDto.QuestionText,
                });

                return Ok(question);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteQuestion([FromQuery] int id)
        {
            try
            {
                var question = await questionService.GetById(id);

                if (ControllerValidator.GetUserIdFromClaims(User.Claims) != question.PostedById)
                {
                    return Unauthorized(
                        new ErrorModel(StatusCodes.Status401Unauthorized, "Unauthorized access to the resource")
                    );
                }

                await questionService.DeleteById(id);

                return Ok(question);
            }
            catch (EntityNotFoundException)
            {
                return NotFound(
                                   new ErrorModel(StatusCodes.Status404NotFound, "Resource not found")
                                                  );
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
