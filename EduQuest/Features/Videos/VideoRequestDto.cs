using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EduQuest.Features.Videos
{
    [ExcludeFromCodeCoverage]
    public class VideoRequestDto
    {
        
        [Required(ErrorMessage = "ContentId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "ContentId must be greater than 0")]
        public int ContentId { get; set; }

        public int DurationHours { get; set; }

        public int DurationMinutes { get; set; }

        public int DurationSeconds { get; set; }

        public string Url { get; set; }
    }
}