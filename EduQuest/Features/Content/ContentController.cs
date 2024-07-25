using EduQuest.Features.Content.Dto;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest.Features.Content
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController(IContentService contentService) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<List<ContentDto>>> GetContentsForSection(int sectionId)
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

        [HttpPost]
        public async Task<ActionResult<ContentDto>> CreateContentForSection([FromBody] ContentDto request)
        {
            try
            {
                var content = await contentService.Add(request);

                return Ok(content);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
