using EduQuest.Features.Auth.Exceptions;
using EduQuest.Features.Course;
using System.Security.Claims;

namespace EduQuest.Commons
{
    public class ControllerValidator(ICourseService courseService)
    {
        public void ValidateUserPrivilegeForEducator(IEnumerable<Claim> claims, int educatorId)
        {

            var enumerable = claims as Claim[] ?? claims.ToArray();
            var usrId = enumerable.FirstOrDefault(c => c.Type == "uid")?.Value;
            var role = enumerable.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (role != "Educator")
            {
                throw new UnAuthorisedUserExeception();
            }
            else if (int.Parse(usrId) != educatorId)
            {
                throw new UnAuthorisedUserExeception();
            }

            return;

        }
    }
}
