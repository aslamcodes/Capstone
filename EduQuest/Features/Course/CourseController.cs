using EduQuest.Commons;
using EduQuest.Features.Course.Dto;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest.Features.Course
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController(ICourseService courseService) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(CourseDTO), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ErrorModel))]
        public async Task<ActionResult<Course>> CreateCourse([FromBody] CourseDTO request)
        {
            try
            {
                var course = await courseService.Add(request);

                return Ok(course);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
