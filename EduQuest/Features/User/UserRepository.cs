using EduQuest.Commons;

namespace EduQuest.Features.User
{
    public class UserRepo(EduQuestContext context) : BaseRepo<int, User>(context);
}
