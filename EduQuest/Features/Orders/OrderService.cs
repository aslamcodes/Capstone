
using AutoMapper;
using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Courses;

namespace EduQuest.Features.Orders
{
    public class OrderService(IRepository<int, Order> orderRepo, ICourseRepo courseRepo, IMapper mapper) : IOrderService
    {
        public async Task<OrderDto> CancelOrder(int orderId)
        {
            var order = await orderRepo.GetByKey(orderId);
            order.OrderStatus = OrderStatusEnum.Cancelled;
            var updatedOrder = await orderRepo.Update(order);
            return mapper.Map<OrderDto>(updatedOrder);
        }

        public async Task<OrderDto> CompleteOrder(int orderId)
        {
            var order = await orderRepo.GetByKey(orderId);
            order.OrderStatus = OrderStatusEnum.Completed;
            var updatedOrder = await orderRepo.Update(order);
            return mapper.Map<OrderDto>(updatedOrder);
        }

        public async Task<OrderDto> CreateOrder(OrderRequestDto orderRequest)
        {
            var course = await courseRepo.GetByKey(orderRequest.OrderedCourse);

            if (course.EducatorId == orderRequest.UserId)
            {
                throw new CannotPlaceOrderException("Educator cannot buy his own course");
            }

            var CoursePrice = course.Price;

            var newOrder = new Order()
            {
                OrderStatus = OrderStatusEnum.Pending,
                CreatedAt = DateTime.Now,
                UserId = orderRequest.UserId,
                Price = CoursePrice,
                OrderedCourseId = course.Id,
            };

            var order = await orderRepo.Add(newOrder);

            return mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> GetOrderById(int id)
        {
            var order = await orderRepo.GetByKey(id);

            return mapper.Map<OrderDto>(order);
        }
    }
}
