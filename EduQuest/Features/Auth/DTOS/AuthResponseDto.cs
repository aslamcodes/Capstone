namespace EduQuest.Features.Auth.DTOS
{
    public class AuthResponseDto
    {
        public string Token { get; set; }

        public string Id { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsEducator { get; set; }
    }
}
