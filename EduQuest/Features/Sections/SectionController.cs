using EduQuest.Commons;
using EduQuest.Features.Auth.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest.Features.Sections
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController(ISectionService sectionService, ControllerValidator validator) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<SectionDto>> GetSectionsForCourse([FromQuery] int courseId)
        {
            try
            {
                var sections = await sectionService.GetSectionForCourse(courseId);

                return Ok(sections);

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

                return section;
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

    }
}
