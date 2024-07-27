using EduQuest.Commons;
using EduQuest.Features.Auth.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace EduQuest.Features.Videos
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController(ControllerValidator validator, IVideoService videoService) : Controller
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<VideoDto>> GetVideoForContent(int contentId)
        {
            try
            {
                await validator.ValidateStudentPrivilegeForContent(User.Claims, contentId);

                var video = await videoService.GetByContentId(contentId);

                return Ok(video);
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

                throw;
            }
        }

        [HttpPost]
        [Authorize("Educator")]
        public async Task<ActionResult<VideoDto>> UploadVideoForContent([FromBody] VideoDto video)
        {
            try
            {

                await validator.ValidateEducatorPrivilegeForContent(User.Claims, video.ContentId);

                var addedVideo = await videoService.Add(video);

                return Ok(addedVideo);
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

                throw;
            }
        }

    }
}
