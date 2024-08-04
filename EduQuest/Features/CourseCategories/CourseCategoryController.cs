using EduQuest.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest.Features.CourseCategories
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseCategoryController(ICategoryService categoryService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IList<CourseCategory>>> GetCourseCategories()
        {
            try
            {
                var categories = await categoryService.GetAll();

                return Ok(categories);
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
