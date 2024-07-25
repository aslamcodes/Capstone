namespace EduQuest.Features.Sections
{
    public class SectionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CourseId { get; set; }
        public int OrderId { get; set; }
    }
}