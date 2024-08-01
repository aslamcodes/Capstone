
namespace EduQuest.Features.Orders
{
    public interface IOrderService
    {
        Task<OrderDto> CancelOrder(int orderId);
        Task<OrderDto> CreateOrder(OrderRequestDto orderRequest);
        Task<OrderDto> GetOrderById(int id);
        Task<OrderDto> CompleteOrder(int orderId);
        Task<List<OrderDto>> GetOrdersForUser(int userId);
        Task<bool> PendingOrderExists(int userId, int orderedCourse);
    }
}