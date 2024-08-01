using EduQuest.Commons;
using EduQuest.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduQuest.Features.Videos
{
    public class VideoRepo(EduQuestContext context) : BaseRepo<int, Video>(context), IVideoRepo
    {
        public async Task<Video> GetByContentId(int contentId)
        {
            var video = await context.Videos.AsNoTracking().FirstOrDefaultAsync(video => video.ContentId == contentId) ?? throw new EntityNotFoundException("Video not found for the content");

            return video;
        }
    }
}
