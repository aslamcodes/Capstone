using AutoMapper;
using Azure;
using Azure.Storage.Blobs;
using EduQuest.Commons;
using EduQuest.Features.Auth.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest.Features.Users
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(
        IUserService userService,
        IMapper mapper,
        IControllerValidator validator,
        BlobServiceClient blobServiceClient,
        ILogger<UserController> _logger) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserProfileDto>> GetUserProfile([FromQuery] int userId)
        {
            try
            {
                var userProfile = await userService.GetById(userId);
                return Ok(userProfile);
            }
            catch (EntityNotFoundException ex)
            {
                return BadRequest(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<UserProfileDto>> UpdateUserProfile([FromBody] UserProfileUpdateDto userProfile)
        {
            try
            {
                var updatedUserProfile = await userService.UpdateProfileEntries(userProfile);
                return Ok(updatedUserProfile);
            }
            catch (EntityNotFoundException ex)
            {
                return BadRequest(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("Become-Educator")]
        [Authorize]
        public async Task<ActionResult<UserProfileDto>> BecomeEducator([FromQuery] int userId)
        {
            try
            {
                await validator.ValidateUserPrivilageForUserId(User.Claims, userId);
                var user = await userService.MakeEducator(userId);
                return Ok(user);
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

        [HttpPut("User-Profile")]
        [Authorize]
        public async Task<ActionResult<UserProfileDto>> UploadUserProfile([FromForm] IFormFile file)
        {
            try
            {
                var userId = validator.GetUserIdFromClaims(User.Claims);
                var profileContainer = blobServiceClient.GetBlobContainerClient("profiles");
                var blob = profileContainer.GetBlobClient($"{userId}-profile.jpg");
                if (await blob.ExistsAsync())
                {
                    await blob.DeleteAsync();
                }

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    await blob.UploadAsync(memoryStream);
                }

                var fileUrl = blob.Uri.AbsoluteUri;
                var user = await userService.GetById(userId);
                user.ProfilePictureUrl = fileUrl;
                var updatedUser = await userService.UpdateProfile(user);
                return Ok(updatedUser);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You are not authorized to perform this action.");
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Unable to access storage. Please try again later.");
            }
            catch (RequestFailedException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while processing your request. Please try again later.");
            }
            catch (IOException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while processing the file. Please try again.");
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    "The request could not be processed. Please check your input and try again.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpGet("Educator-Profile")]
        [Authorize]
        public async Task<ActionResult<EducatorProfileDto>> GetEducatorProfile([FromQuery] int educatorId)
        {
            try
            {
                var educatorProfile = await userService.GetById(educatorId);
                return Ok(mapper.Map<EducatorProfileDto>(educatorProfile));
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