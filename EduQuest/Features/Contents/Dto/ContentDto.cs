using System.ComponentModel.DataAnnotations;

namespace EduQuest.Features.Contents.Dto
{
    public class ContentDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int SectionId { get; set; }
        public int OrderIndex { get; set; }

        [RegularExpression("^(Video|Article)$", ErrorMessage = "Invalid value. Allowed values are: Video, Article.")]
        public required string ContentType { get; set; }

    }
}