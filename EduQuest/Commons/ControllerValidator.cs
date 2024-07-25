using EduQuest.Features.Auth.Exceptions;
using EduQuest.Features.Content;
using EduQuest.Features.Course;
using EduQuest.Features.Sections;
using System.Security.Claims;

namespace EduQuest.Commons
{
    public class ControllerValidator(ICourseService courseService, IContentService contentService, ISectionService sectionService)
    {
        public async Task ValidateEducatorPrivilegeForCourse(IEnumerable<Claim> claims, int courseId)
        {
            var userId = GetUserIdFromClaims(claims);

            var course = await courseService.GetById(courseId);

            if (course.EducatorId != userId) throw new UnAuthorisedUserExeception();

            return;

        }

        public async Task ValidateEducatorPrivilegeForContent(IEnumerable<Claim> claims, int contentId)
        {

            var content = await contentService.GetById(contentId);

            var section = await sectionService.GetById(content.SectionId);

            var course = await courseService.GetById(section.CourseId);

            var userId = GetUserIdFromClaims(claims);

            if (course.EducatorId != userId) throw new UnAuthorisedUserExeception();

            return;

        }

        public async Task ValidateEducatorPrivilegeForSection(IEnumerable<Claim> claims, int sectionId)
        {
            var userId = GetUserIdFromClaims(claims);

            var section = await sectionService.GetById(sectionId);

            var course = await courseService.GetById(section.CourseId);

            if (userId != course.EducatorId) throw new UnAuthorisedUserExeception();

            return;
        }

        static int GetUserIdFromClaims(IEnumerable<Claim> claims)
        {

            var claimArr = claims as Claim[] ?? claims.ToArray();

            var usrId = claimArr.FirstOrDefault(c => c.Type == "uid")?.Value;

            return usrId == null ? throw new UnAuthorisedUserExeception() : int.Parse(usrId);
        }

        public Task ValidateEducatorPrevilege(IEnumerable<Claim> claims, int educatorId)
        {
            var userId = GetUserIdFromClaims(claims);

            if (userId != educatorId) throw new UnAuthorisedUserExeception();

            return Task.CompletedTask;
        }
    }
}
