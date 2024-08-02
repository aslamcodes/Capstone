using AutoMapper;
using EduQuest.Commons;
using EduQuest.Features.Auth.Exceptions;
using EduQuest.Features.Courses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest.Features.Reviews
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController(IReviewService reviewService, ICourseService courseService, ControllerValidator validator, IMapper mapper) : Controller
    {
        [HttpGet("For-Courses")]
        public async Task<IActionResult> GetReviewsByCourse(int courseId)
        {
            try
            {
                var reviews = await reviewService.GetReviewsByCourse(courseId);

                return Ok(reviews);
            }
         
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReview(ReviewRequestDto reviewDto)
        {
            try
            {
                await validator.ValidateStudentPrivilegeForCourse(User.Claims, reviewDto.CourseId);

                var review = await reviewService.Add(mapper.Map<ReviewDto>(reviewDto));

                return Ok(review);
            }
            catch (UnAuthorisedUserExeception ex)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized, ex.Message));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
         
        }
    }
}
