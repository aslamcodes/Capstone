using EduQuest.Commons;

namespace EduQuest.Features.Sections
{
    public interface ISectionService : IBaseService<Section, SectionDto>
    {
        Task<IList<Section>> DeleteSectionsForCourse(int courseId);
        Task<IList<SectionDto>> GetSectionForCourse(int courseId);
    }
}