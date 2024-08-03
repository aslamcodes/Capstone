using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EduQuest.Features.Articles
{
    [ExcludeFromCodeCoverage]
    public class ArticleDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Article ID must be a positive integer.")]
        public int Id { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Content ID must be a positive integer.")]
        public int ContentId { get; set; }
        
        public string Title { get; set; }

        public string Description { get; set; }

        public string Body { get; set; }

    }
}