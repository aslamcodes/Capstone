using System.ComponentModel.DataAnnotations;

namespace EduQuest.Features.Reviews
{
    public class ReviewRequestDto
    {
        [Required(ErrorMessage = "CourseId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "CourseId must be greater than 0")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        
        public string ReviewText { get; set; }

        [Required(ErrorMessage = "ReviewedById is required")]
        [Range(1, int.MaxValue, ErrorMessage = "ReviewedById must be greater than 0")]
        public int ReviewedById { get; set; }
    }
}