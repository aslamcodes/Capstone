using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.Courses
{
    public interface ICourseRepo : IRepository<int, Course>
    {
        Task<List<Course>> GetBySearch(string query);
        Task<List<Course>> GetByStatus(CourseStatusEnum status);
    }
}