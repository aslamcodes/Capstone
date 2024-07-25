using AutoMapper;
using EduQuest.Commons;

namespace EduQuest.Features.Sections
{
    public class SectionService(IRepository<int, Section> sectionRepo, IMapper mapper) : BaseService<Section, SectionDto>(sectionRepo, mapper), ISectionService
    {
        public async Task<IList<SectionDto>> GetSectionForCourse(int courseId)
        {
            var sections = await sectionRepo.GetAll();

            return sections.Where(s => s.CourseId == courseId)
                .Select(mapper.Map<SectionDto>)
                .ToList();
        }
    }
}
