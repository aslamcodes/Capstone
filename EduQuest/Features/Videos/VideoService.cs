using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.Videos
{
    [ExcludeFromCodeCoverage]
    public class VideoService(IVideoRepo videoRepo, IMapper mapper) : BaseService<Video, VideoDto>(videoRepo, mapper), IVideoService
    {
        public async Task<VideoDto> GetByContentId(int contentId)
        {
            var video = await videoRepo.GetByContentId(contentId);

            return mapper.Map<VideoDto>(video);
        }
    }
}
