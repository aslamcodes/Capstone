using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Users;

namespace EduQuest.Entities
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

        public string ProfilePictureUrl { get; set; }
        
        public UserStatusEnum Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public IEnumerable<Course> CoursesEnrolled { get; set; }
        public IEnumerable<Order> CoursesOrdered { get; set; }

        public IEnumerable<Course> CoursesCreated { get; set; }

        public IEnumerable<Note> Notes { get; set; }


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