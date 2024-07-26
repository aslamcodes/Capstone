using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.Videos
{
    public interface IVideoRepo : IRepository<int, Video>
    {
        Task<Video> GetByContentId(int contentId);
    }
}