using EduQuest.Commons;

namespace EduQuest.Features.Videos
{
    public interface IVideoService : IBaseService<VideoDto, VideoDto>
    {
        Task<VideoDto> GetByContentId(int contentId);
    }
}