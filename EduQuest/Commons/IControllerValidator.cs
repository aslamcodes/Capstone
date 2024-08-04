using System.Security.Claims;

namespace EduQuest.Commons;

public interface IControllerValidator
{
    Task ValidateEducatorPrivilegeForCourse(IEnumerable<Claim> claims, int courseId);
    Task ValidateEducatorPrivilegeForContent(IEnumerable<Claim> claims, int contentId);
    Task ValidateEducatorPrivilegeForSection(IEnumerable<Claim> claims, int sectionId);
    Task ValidateEducatorPrevilege(IEnumerable<Claim> claims, int educatorId);
    Task ValidateUserPrivilageForOrder(IEnumerable<Claim> claims, int orderId);
    Task ValidateUserPrivilageForUserId(IEnumerable<Claim> claims, int userID);
    Task ValidateStudentPrivilegeForCourse(IEnumerable<Claim> claims, int courseId);
    Task ValidateUserPrivilegeForContent(IEnumerable<Claim> claims, int contentId);   
    int GetUserIdFromClaims(IEnumerable<Claim> claims);
}