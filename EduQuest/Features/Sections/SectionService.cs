using AutoMapper;
using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.Sections
{
    public class SectionService(ISectionRepo sectionRepo, IMapper mapper) : BaseService<Section, SectionDto>(sectionRepo, mapper), ISectionService
    {
        public async Task<IList<Section>> DeleteSectionsForCourse(int courseId)
        {
            return await sectionRepo.DeleteByCourse(courseId);
        }

        public async Task<IList<SectionDto>> GetSectionForCourse(int courseId)
        {
            var sections = await sectionRepo.GetAll();

            return sections.Where(s => s.CourseId == courseId)
                .Select(mapper.Map<SectionDto>)
                .OrderBy(s => s.OrderId)
                .ToList();
        }
    }
}
