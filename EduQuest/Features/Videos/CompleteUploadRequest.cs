using System.Diagnostics.CodeAnalysis;

namespace EduQuest.Features.Videos
{
    [ExcludeFromCodeCoverage]
    public class CompleteUploadRequest
    {
        public int ContentId { get; set; }
        public string FileName { get; set; }
    }
}