using AutoMapper;
using Azure.Storage.Blobs;
using EduQuest.Commons;
using EduQuest.Features.Questions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest.Features.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService, IMapper mapper, ControllerValidator validator, BlobServiceClient blobServiceClient) : ControllerBase
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
            catch (Exception)
            {

                throw;
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
            catch (Exception)
            {

                throw;
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
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut("User-Profile")]
        [Authorize]
        public async Task<ActionResult<UserProfileDto>> UploadUserProfile([FromForm] IFormFile file)
        {

            int userId = ControllerValidator.GetUserIdFromClaims(User.Claims);
            BlobContainerClient profileContainer = blobServiceClient.GetBlobContainerClient("profiles");

            BlobClient blob = profileContainer.GetBlobClient($"{userId}-profile.jpg");

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

            UserProfileDto updatedUser;

            user.ProfilePictureUrl = fileUrl;

            updatedUser = await userService.UpdateProfile(user);



            return Ok(updatedUser);
        }
        ///api/User/Educator-Profile? educatorId =${educatorId
        [HttpGet("Educator-Profile")]
        [Authorize]
        public async Task<ActionResult<EducatorProfileDto>> GetEducatorProfile([FromQuery] int educatorId)
        {
            try
            {
                var educatorProfile = await userService.GetById(educatorId);

                return Ok(mapper.Map<EducatorProfileDto>(educatorProfile));
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
