namespace EduQuest.Features.Contents.Dto
{
    public class ContentDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int SectionId { get; set; }
        public int OrderIndex { get; set; }
        public ContentTypeEnum ContentType { get; set; }
    }
}