using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Auth.Exceptions;
using EduQuest.Features.Courses;
using EduQuest.Features.Orders;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest.Features.Payments
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController(IPaymentService paymentService, IOrderService orderService, ICourseService courseService, IControllerValidator validator) : ControllerBase
    {
        [HttpPost("Make-Payment")]
        public async Task<ActionResult<Payment>> MakePaymentForOrder([FromQuery] int orderId)
        {
            try
            {
                await validator.ValidateUserPrivilageForOrder(User.Claims, orderId);

                var payment = await paymentService.MakePaymentForOrder(orderId);

                var order = await orderService.CompleteOrder(orderId);

                var enroll = await courseService.EnrollStudentIntoCourse(order.UserId, order.OrderedCourseId);

                return Ok(payment);
            }
            catch (CannotMakePaymentException ex)
            {
                return BadRequest(new ErrorModel(StatusCodes.Status400BadRequest, ex.Message));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (UnAuthorisedUserExeception ex)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized, ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

        }


    }
}
