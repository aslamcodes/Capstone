using EduQuest.Features.Auth.Exceptions;
using EduQuest.Features.Contents;
using EduQuest.Features.Courses;
using EduQuest.Features.Orders;
using EduQuest.Features.Sections;
using System.Security.Claims;

namespace EduQuest.Commons
{
    public class ControllerValidator(ICourseService courseService,
                                     IOrderService orderService,
                                     IContentService contentService,
                                     ISectionService sectionService) : IControllerValidator
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

        public int GetUserIdFromClaims(IEnumerable<Claim> claims)
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

        public async Task ValidateUserPrivilageForOrder(IEnumerable<Claim> claims, int orderId)
        {
            var userId = GetUserIdFromClaims(claims);

            var OrderId = await orderService.GetOrderById(orderId);

            if (OrderId.UserId != userId) throw new UnAuthorisedUserExeception();

            return;
        }

        public Task ValidateUserPrivilageForUserId(IEnumerable<Claim> claims, int userID)
        {
            var UserIdClaim = GetUserIdFromClaims(claims);

            if (userID != UserIdClaim) throw new UnAuthorisedUserExeception();

            return Task.CompletedTask;
        }

        public async Task ValidateStudentPrivilegeForCourse(IEnumerable<Claim> claims, int courseId)
        {
            var userId = GetUserIdFromClaims(claims);

            var enrolledCourses = await courseService.GetCoursesForStudent(userId);

            var course = await courseService.GetById(courseId);

            if (enrolledCourses.Any(c => c.Id == course.Id)) return;

            throw new UnAuthorisedUserExeception();
        }

        public async Task ValidateUserPrivilegeForContent(IEnumerable<Claim> claims, int contentId)
        {
            var userId = GetUserIdFromClaims(claims);

            var enrolledCourses = await courseService.GetCoursesForStudent(userId);


            var content = await contentService.GetById(contentId);

            var section = await sectionService.GetById(content.SectionId);

            var course = await courseService.GetById(section.CourseId);


            if (course.EducatorId == userId) return;

            if (enrolledCourses.Any(c => c.Id == course.Id)) return;

            throw new UnAuthorisedUserExeception();
        }
    }
}
