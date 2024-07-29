namespace EduQuest.Features.Courses
{
    public record ValidityCriteria
    {
        public bool IsPassed { get; set; }

        public string Criteria { get; set; }
    }
}
