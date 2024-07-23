namespace EduQuest.Features.Auth.DTOS
{
    public class AuthResponseDto
    {
        public string Token { get; set; }

        public string Email { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsEducator { get; set; }
    }
}
