using System.Diagnostics.CodeAnalysis;

namespace EduQuest.Features.Videos
{
    [ExcludeFromCodeCoverage]
    public class GetUploadUrlRequest
    {
        public int ContentId { get; set; }
        public string FileName { get; set; }
    }
}