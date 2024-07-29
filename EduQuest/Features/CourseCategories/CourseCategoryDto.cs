using EduQuest.Entities;

namespace EduQuest.Features.CourseCategories
{
    public class CourseCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}