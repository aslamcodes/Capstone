using AutoMapper;
using EduQuest.Commons;
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
            var reviews = await reviewService.GetReviewsByCourse(courseId);

            return Ok(reviews);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReview(ReviewRequestDto reviewDto)
        {
            await validator.ValidateStudentPrivilegeForCourse(User.Claims, reviewDto.CourseId);

            var review = await reviewService.Add(mapper.Map<ReviewDto>(reviewDto));

            return Ok(review);
        }
    }
}
