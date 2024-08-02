using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EduQuest.Features.Courses.Dto
{
    [ExcludeFromCodeCoverage]
    public class CourseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        [RegularExpression("^[^|]+(\\|[^|]+)*$", ErrorMessage = "Invalid Course Objectives")]
        public string? CourseObjective { get; set; }

        [RegularExpression("^[^|]+(\\|[^|]+)*$", ErrorMessage = "Invalid Prerequisites")]
        public string? Prerequisites { get; set; }

        [RegularExpression("^[^|]+(\\|[^|]+)*$", ErrorMessage = "Invalid TargetAudience")]
        public string? TargetAudience { get; set; }

        public int CourseCategoryId { get; set; }

        public string? CourseThumbnailPicture { get; set; }

        public int EducatorId { get; set; }
        public float Price { get; set; }

        public string CourseStatus { get; set; }

        [RegularExpression("^(Begginer|Intermediate|Advanced)$", ErrorMessage = "Invalid value. Allowed values are: Begginer, Intermediate, Advanced.")]
        public string Level { get; set; }
    }
}
