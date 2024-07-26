using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.Payments
{
    public class PaymentRepo(EduQuestContext context) : BaseRepo<int, Payment>(context)
    {
    }
}
