using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EduQuest.Features.Orders
{
    [ExcludeFromCodeCoverage]
    public class OrderRequestDto
    {
        [Required(ErrorMessage = "UserId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be a positive integer.")]
        public int UserId { get; set; }
        
        [Required(ErrorMessage = "CourseId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "CourseId must be a positive integer.")]
        public int OrderedCourse { get; set; }

    }
}
