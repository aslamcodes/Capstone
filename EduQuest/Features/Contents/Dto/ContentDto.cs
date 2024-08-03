using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EduQuest.Features.Contents.Dto
{
    [ExcludeFromCodeCoverage]
    public class ContentDto
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters.")]
        public string Title { get; set; }
        
        [Required(ErrorMessage = "SectionId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "SectionId must be a positive integer.")]
        public int SectionId { get; set; }
        
        public int OrderIndex { get; set; }
        
        [Required(ErrorMessage = "ContentType is required.")]
        [RegularExpression("^(Video|Article)$", ErrorMessage = "Invalid value. Allowed values are: Video, Article.")]
        public required string ContentType { get; set; }

    }
}