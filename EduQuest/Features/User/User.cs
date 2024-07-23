
using EduQuest.Commons;
using EduQuest.Features.User;

namespace EduQuest.Features.User
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

        public string? Phone { get; set; }

        public byte[] Password { get; set; }
        public byte[] PasswordHashKey { get; set; }

        public bool IsEducator { get; set; } = false;

        public bool IsAdmin { get; set; } = false;

        public UserStatusEnum Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

public static class UserExtensions
{
    public static bool IsPasswordCorrect(this User user, byte[] password)
    {
        for (int i = 0; i < password.Length; i++)
        {
            if (password[i] != user.Password[i])
            {
                return false;
            }
        }
        return true;
    }
}