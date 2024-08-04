using System.Diagnostics.CodeAnalysis;

namespace EduQuest.Features.Videos
{
    [ExcludeFromCodeCoverage]
    public class VideoDto
    {
        public int Id { get; set; }
        public int ContentId { get; set; }

        public int DurationHours { get; set; }

        public int DurationMinutes { get; set; }

        public int DurationSeconds { get; set; }

        public string Url { get; set; }

    }
}