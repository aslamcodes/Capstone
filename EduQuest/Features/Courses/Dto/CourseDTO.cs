using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EduQuest.Features.Courses.Dto
{
    [ExcludeFromCodeCoverage]
    public class CourseDTO
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive integer.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 200 characters.")]
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

        [Required(ErrorMessage = "Educator Id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Educator Id must be a positive integer.")]
        public int EducatorId { get; set; }
        public float Price { get; set; }
        public string CourseStatus { get; set; }

        [RegularExpression("^(Beginner|Intermediate|Advanced)$",
            ErrorMessage = "Invalid value. Allowed values are: Beginner, Intermediate, Advanced.")]
        public string Level { get; set; }
    }
}