using EduQuest.Commons;
using EduQuest.Features.Auth.Exceptions;
using EduQuest.Features.Courses.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest.Features.Student
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController(IStudentService studentService, IControllerValidator validator) : ControllerBase
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

        [HttpGet("user-owns-course")]
        [Authorize]
        public async Task<ActionResult<bool>> UserOwnsCourse([FromQuery] int courseId)
        {
            try
            {

                var userOwnsCourse = await studentService.UserOwnsCourse(validator.GetUserIdFromClaims(User.Claims), courseId);

                return Ok(userOwnsCourse);
            }
            catch (UnAuthorisedUserExeception ex)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized, ex.Message));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        [HttpGet("user-manages-course")]
        [Authorize]
        public async Task<ActionResult<bool>> UserManagesCourse([FromQuery] int courseId)
        {
            try
            {

                var userOwnsCourse = await studentService.UserManagesCourse(validator.GetUserIdFromClaims(User.Claims), courseId);

                return Ok(userOwnsCourse);
            }
            catch (UnAuthorisedUserExeception ex)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized, ex.Message));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }


        [HttpGet("home-courses")]
        public async Task<ActionResult<List<CourseDTO>>> GetHomeCourses([FromQuery] int studentId)
        {
            try
            {
                var homeCourses = await studentService.GetHomeCourses(studentId);

                return Ok(homeCourses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }
    }
}
