namespace EduQuest.Features.Content.Dto
{
    public class ContentResponseDto
    {
        public string Title { get; set; }
        public int SectionId { get; set; }
        public int OrderIndex { get; set; }
        public ContentTypeEnum ContentType { get; set; }
    }
}