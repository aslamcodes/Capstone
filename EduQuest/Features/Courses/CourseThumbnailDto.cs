using System.Diagnostics.CodeAnalysis;

namespace EduQuest.Features.Courses
{
    [ExcludeFromCodeCoverage]
    public class CourseThumbnailDto
    {
        public int CourseId { get; set; }


        public IFormFile file { get; set; }
    }
}