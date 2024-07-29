namespace EduQuest.Features.Courses.Dto
{
    public class ValidityResponseDto
    {
        public bool IsValid { get; set; }

        public List<ValidityCriteria> Criterias { get; set; }
    }
}
