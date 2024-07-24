using EduQuest.Commons;
using EduQuest.Features.Auth.Exceptions;
using EduQuest.Features.Course.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest.Features.Course
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController(ICourseService courseService, ControllerValidator validator) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(CourseDTO), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorModel))]
        [Authorize(Policy = "Educator")]
        public async Task<ActionResult<Course>> CreateCourse([FromBody] CourseDTO request)
        {
            try
            {
                validator.ValidateUserPrivilegeForEducator(User.Claims, request.EducatorId);
                var course = await courseService.Add(request);

                return Ok(course);
            }
            catch (UnAuthorisedUserExeception)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized, "Not having enought permissions"));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
