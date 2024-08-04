using EduQuest.Commons;
using EduQuest.Features.Auth.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest.Features.Articles
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController(IArticleService articleService, IControllerValidator validator) : ControllerBase
    {
        [HttpGet("ForContent")]
        public async Task<ActionResult<ArticleDto>> GetArticleByContentId([FromQuery] int contentId)
        {
            try
            {
                await validator.ValidateUserPrivilegeForContent(User.Claims, contentId);

                var article = await articleService.GetByContentId(contentId);

                return Ok(article);
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


        [HttpPut]
        [Authorize(Policy = "Educator")]
        public async Task<ActionResult<ArticleDto>> UpdateArticle([FromBody] ArticleDto article)
        {
            {
                try
                {
                    await validator.ValidateEducatorPrivilegeForContent(User.Claims, article.ContentId);

                    var updatedArticle = await articleService.Update(article);

                    return Ok(updatedArticle);
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
}
