using System.ComponentModel.DataAnnotations;

namespace EduQuest.Features.Course.Dto
{
    public class CourseDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int EducatorId { get; set; }

        public float Price { get; set; }

        [RegularExpression("^(Begginer|Intermediate|Advanced)$", ErrorMessage = "Invalid value. Allowed values are: Begginer, Intermediate, Advanced.")]
        public string Level { get; set; }
    }
}
