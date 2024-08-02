using System.Diagnostics.CodeAnalysis;

namespace EduQuest.Features.Courses.Dto
{
    [ExcludeFromCodeCoverage]
    public class ValidityResponseDto
    {
        public bool IsValid { get; set; }

        public List<ValidityCriteria> Criterias { get; set; }
    }
}
