using System.ComponentModel.DataAnnotations;

namespace EduQuest.Features.Auth.DTOS
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        [MaxLength(50, ErrorMessage = "Password can't be longer than 50 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name can't be longer than 50 characters")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Last name can't be longer than 50 characters")]
        public string LastName { get; set; }
    }
}
