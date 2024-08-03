using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EduQuest.Features.Courses
{
    [ExcludeFromCodeCoverage]
    public class CourseRequestDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 200 characters.")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "EducatorId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "EducatorId must be a positive integer.")]
        public int EducatorId { get; set; }

        public int CourseCategoryId { get; set; }

        public float Price { get; set; }

        [RegularExpression("^(Beginner|Intermediate|Advanced)$",
            ErrorMessage = "Invalid value. Allowed values are: Beginner, Intermediate, Advanced.")]
        public string Level { get; set; }
    }
}