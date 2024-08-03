using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EduQuest.Features.Answers
{
    [ExcludeFromCodeCoverage]
    public class AnswerRequestDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "QuestionId must be a positive integer.")]
        public int QuestionId { get; set; }
        
        [Required]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "AnswerText must be between 1 and 1000 characters.")]
        public string AnswerText { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "AnsweredById must be a positive integer.")]
        public int AnsweredById { get; set; }

    }
}