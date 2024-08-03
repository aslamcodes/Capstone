using System.ComponentModel.DataAnnotations;

namespace EduQuest.Features.Sections
{
    public class SectionDto
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 200 characters")]
        public string Name { get; set; }
        
        [Required]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 200 characters")]
        public string Description { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "CourseId must be greater than 0")]
        public int CourseId { get; set; }
        public int OrderId { get; set; }
    }
}