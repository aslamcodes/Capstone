using EduQuest.Commons;
using EduQuest.Features.Auth.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest.Features.Orders
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrderService orderService, IControllerValidator validator) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<OrderDto>> GetOrderDetails([FromQuery] int orderId)
        {
            try
            {
                await validator.ValidateUserPrivilageForOrder(User.Claims, orderId);

                var order = await orderService.GetOrderById(orderId);

                return Ok(order);
            }
            catch (UnAuthorisedUserExeception)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized,
                    "Unauthorized access to the resource"));
            }
            catch (EntityNotFoundException)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, "Resource not found"));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderDto>> PlaceOrder([FromBody] OrderRequestDto orderRequest)
        {
            try
            {
                await validator.ValidateUserPrivilageForUserId(User.Claims, orderRequest.UserId);

                var order = await orderService.CreateOrder(orderRequest);

                return Ok(order);
            }
            catch (CannotPlaceOrderException ex)
            {
                return BadRequest(new ErrorModel(StatusCodes.Status400BadRequest, ex.Message));
            }
            catch (UnAuthorisedUserExeception)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized,
                    "Unauthorized access to the resource"));
            }
            catch (EntityNotFoundException)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, "Resource not found"));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("Cancel")]
        [Authorize]
        public async Task<ActionResult<OrderDto>> CancelOrder([FromQuery] int orderId)
        {
            try
            {
                await validator.ValidateUserPrivilageForOrder(User.Claims, orderId);

                var order = await orderService.CancelOrder(orderId);

                return Ok(order);
            }
            catch (UnAuthorisedUserExeception)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized,
                    "Unauthorized access to the resource"));
            }
            catch (EntityNotFoundException)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, "Resource not found"));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("user-orders")]
        [Authorize]
        public async Task<ActionResult<List<OrderDto>>> GetUserOrders()
        {
            try
            {
                var userId = validator.GetUserIdFromClaims(User.Claims);

                var orders = await orderService.GetOrdersForUser(userId);

                return Ok(orders);
            }

            catch (Exception)
            {
                return StatusCode(500);
            }

        }

    }
}
