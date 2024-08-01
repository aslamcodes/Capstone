using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Questions;

namespace EduQuest.Features.Users
{
    public interface IUserService : IBaseService<User, UserProfileDto>
    {
        Task<User> AddAsync(User user);
        Task<User> GetByEmailAsync(string email);
        Task<UserProfileDto> MakeEducator(int userId);
        Task<UserProfileDto> UpdateProfile(UserProfileDto user);
        Task<UserProfileDto> UpdateProfileEntries(UserProfileUpdateDto userProfile);
    }
}
