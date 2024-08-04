using EduQuest.Commons;
using EduQuest.Features.Auth.Exceptions;
using EduQuest.Features.Contents;
using EduQuest.Features.Contents.Dto;
using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest.Features.Sections
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController(ISectionService sectionService, IContentService contentService, IControllerValidator validator) : Controller
    {

        [HttpGet("Contents")]
        public async Task<ActionResult<List<ContentDto>>> GetContentsForSection([FromQuery] int sectionId)
        {
            try
            {
                var contents = await contentService.GetContentBySection(sectionId);

                return Ok(contents);
            }
            catch (Exception)
            {

                throw;
            }
        }


        [Authorize(Policy = "Educator")]
        [HttpGet]
        public async Task<ActionResult<SectionDto>> GetSection([FromQuery] int sectionId)
        {
            try
            {

                if (sectionId == 0)
                {
                    return BadRequest(new ErrorModel(StatusCodes.Status400BadRequest, "Invalid section id"));
                }

                var section = await sectionService.GetById(sectionId);

                return Ok((section));
            }
            catch (EntityNotFoundException)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, "Course not found"));
            }
            catch (Exception)
            {

                throw;
            }
        }


        [Authorize(Policy = "Educator")]
        [HttpPost]
        public async Task<ActionResult<SectionDto>> CreateSection([FromBody] SectionDto request)
        {
            try
            {
                await validator.ValidateEducatorPrivilegeForCourse(User.Claims, request.CourseId);

                var section = await sectionService.Add(request);

                return Ok(section);
            }
            catch (ReferenceConstraintException)
            {
                return BadRequest(new ErrorModel(StatusCodes.Status400BadRequest, "Invalid course id"));
            }
            catch (EntityNotFoundException)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, "Course not found"));
            }
            catch (UnAuthorisedUserExeception)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized, "Unauthorised"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        [Authorize(Policy = "Educator")]
        public async Task<ActionResult<SectionDto>> UpdateSection([FromBody] SectionDto section)
        {

            try
            {
                await validator.ValidateEducatorPrivilegeForSection(User.Claims, section.Id);
                await validator.ValidateEducatorPrivilegeForCourse(User.Claims, section.CourseId);

                var updatedSection = await sectionService.Update(section);

                return Ok(updatedSection);
            }
            catch (ReferenceConstraintException ex)
            {
                return BadRequest(new ErrorModel(StatusCodes.Status400BadRequest, "Invalid course id"));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (UnAuthorisedUserExeception)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized, "Unauthorised"));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

        }

        [Authorize(Policy = "Educator")]
        [HttpDelete]
        public async Task<ActionResult<SectionDto>> DeleteSection([FromQuery] int sectionId)
        {

            try
            {
                await validator.ValidateEducatorPrivilegeForSection(User.Claims, sectionId);

                await contentService.DeleteBySection(sectionId);

                var deletedId = await sectionService.DeleteById(sectionId);

                return Ok(deletedId);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (UnAuthorisedUserExeception)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized, "Unauthorised"));
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
