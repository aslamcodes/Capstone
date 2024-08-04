using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EduQuest.Features.Questions
{
    [ExcludeFromCodeCoverage]
    public class QuestionRequestDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "ContentId must be a positive integer.")]
        public int ContentId { get; set; }

        [Required(ErrorMessage = "QuestionText is required.")]
        public string QuestionText { get; set; }

        [Required(ErrorMessage = "PostedById is required")]
        [Range(1, int.MaxValue, ErrorMessage = "PostedById must be a positive integer.")]
        public int PostedById { get; set; }

    }
}