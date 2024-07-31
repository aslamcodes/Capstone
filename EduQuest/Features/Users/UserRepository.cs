using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.Users
{
    public class UserRepo(EduQuestContext context) : BaseRepo<int, User>(context), IRepository<int, User>
    {

    }
}
