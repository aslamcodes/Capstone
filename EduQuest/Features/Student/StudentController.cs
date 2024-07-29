using EduQuest.Commons;
using EduQuest.Features.Courses.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest.Features.Student
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController(IStudentService studentService) : ControllerBase
    {
        [Authorize]
        [HttpGet("recommended-courses")]
        public async Task<ActionResult<List<CourseDTO>>> GetRecommendedCourses([FromQuery] int studentId)
        {
            try
            {
                var recommendedCourses = await studentService.GetRecommendedCourses(studentId);

                return Ok(recommendedCourses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
    }
}
