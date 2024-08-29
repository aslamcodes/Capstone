using AutoMapper;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using EduQuest.Commons;
using EduQuest.Features.Auth.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
namespace EduQuest.Features.Videos
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController(IControllerValidator validator, IVideoService videoService, BlobServiceClient blobServiceClient, IMapper mapper) : Controller
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<VideoDto>> GetVideoForContent(int contentId)
        {
            try
            {
                await validator.ValidateUserPrivilegeForContent(User.Claims, contentId);

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
        [Authorize(Policy = "Educator")]
        public async Task<ActionResult<VideoDto>> UploadVideoDataForContent([FromBody] VideoRequestDto video)
        {
            try
            {

                await validator.ValidateEducatorPrivilegeForContent(User.Claims, video.ContentId);

                var addedVideo = await videoService.Add(mapper.Map<VideoDto>(video));

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

        [HttpPost("get-upload-url")]
        [Authorize(Policy = "Educator")]
        public async Task<ActionResult<UploadUrlResponse>> GetUploadUrl([FromBody] GetUploadUrlRequest request)
        {
            try
            {
                var containerClient = blobServiceClient.GetBlobContainerClient("videos");
                var blobClient = containerClient.GetBlobClient($"{request.ContentId}-{request.FileName}");
                // Generate SAS token for the blob
                var sasBuilder = new BlobSasBuilder
                {
                    BlobContainerName = containerClient.Name,
                    BlobName = blobClient.Name,
                    Resource = "b",
                    ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
                };
                sasBuilder.SetPermissions(BlobSasPermissions.Write);

                // var accountName = secretClient.GetSecret("StorageAccountName").Value.Value;
                // var accountKey = secretClient.GetSecret("StorageAccountKey").Value.Value;
                //
                // var sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(accountName, accountKey)).ToString();
                var uploadUrl = $"";

                // Create a record in your database
                //await _videoService.InitiateUpload(request.ContentId, request.FileName);

                return Ok(new UploadUrlResponse { UploadUrl = uploadUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while generating the upload URL");
            }
        }

        [HttpPost("complete-upload")]
        [Authorize(Policy = "Educator")]
        public async Task<ActionResult<VideoDto>> CompleteUpload([FromBody] CompleteUploadRequest request)
        {
            try
            {
                await validator.ValidateEducatorPrivilegeForContent(User.Claims, request.ContentId);

                var containerClient = blobServiceClient.GetBlobContainerClient("videos");

                var blobClient = containerClient.GetBlobClient($"{request.ContentId}-{request.FileName}");

                var properties = await blobClient.GetPropertiesAsync();

                var video = await videoService.GetByContentId(request.ContentId);

                video.Url = blobClient.Uri.ToString();

                var updatedVideo = await videoService.Update(video);
                //await _videoService.CompleteUpload(request.ContentId blobClient.Uri.ToString());

                return Ok(updatedVideo);
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
                return StatusCode(500);
            }
        }


    }
}
