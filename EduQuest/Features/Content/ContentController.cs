using EduQuest.Commons;
using EduQuest.Features.Auth.Exceptions;
using EduQuest.Features.Content.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest.Features.Content
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController(IContentService contentService, ControllerValidator controllerValidator) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<ContentDto>> GetContent([FromQuery] int contentId)
        {
            try
            {
                var content = await contentService.GetById(contentId);

                return Ok(content);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        [Authorize(Policy = "Educator")]
        public async Task<ActionResult<ContentDto>> UpdateContent([FromBody] ContentDto content)
        {
            try
            {
                await controllerValidator.ValidateEducatorPrivilegeForContent(User.Claims, content.Id);
                await controllerValidator.ValidateEducatorPrivilegeForSection(User.Claims, content.SectionId);
                var updatedContent = await contentService.Update(content);

                return Ok(updatedContent);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (UnAuthorisedUserExeception ex)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized, ex.Message));
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpDelete]
        [Authorize(Policy = "Educator")]
        public async Task<ActionResult<ContentDto>> DeleteContent([FromQuery] int contentId)
        {
            try
            {
                await controllerValidator.ValidateEducatorPrivilegeForContent(User.Claims, contentId);

                var deletedContent = await contentService.DeleteById(contentId);

                return Ok(deletedContent);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (UnAuthorisedUserExeception ex)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized, ex.Message))
            ;
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = "Educator")]
        public async Task<ActionResult<ContentDto>> CreateContent([FromBody] ContentDto request)
        {
            try
            {
                await controllerValidator.ValidateEducatorPrivilegeForSection(User.Claims, request.SectionId);

                var content = await contentService.Add(request);

                return Ok(content);
            }
            catch (UnAuthorisedUserExeception ex)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized, ex.Message))
            ;
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
