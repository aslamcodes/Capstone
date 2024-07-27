using EduQuest.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest.Features.Articles
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController(IArticleService articleService, ControllerValidator validator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ArticleDto>> GetArticleByContentId(int contentId)
        {
            try
            {
                await validator.ValidateStudentPrivilegeForContent(User.Claims, contentId);

                var article = await articleService.GetByContentId(contentId);

                return Ok(article);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        [Authorize(Policy = "Educator")]
        public async Task<ActionResult<ArticleDto>> CreateArticle([FromBody] ArticleDto article)
        {
            try
            {
                await validator.ValidateEducatorPrivilegeForContent(User.Claims, article.ContentId);

                var addedArticle = await articleService.Add(article);

                return Ok(addedArticle);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized, ex.Message));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
