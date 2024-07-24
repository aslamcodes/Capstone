using Microsoft.AspNetCore.Mvc;

namespace EduQuest.Features.Content
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController(IContentService contentService) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<List<Content>>> GetContentsForSection()
        {
            try
            {
                var contents = await contentService.GetAll();

                return Ok(contents);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
