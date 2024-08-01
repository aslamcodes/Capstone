using EduQuest.Commons;
using EduQuest.Features.Auth.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest.Features.Orders
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrderService orderService, ControllerValidator validator) : ControllerBase
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
            catch (UnAuthorisedUserExeception ex)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized, ex.Message));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (Exception)
            {

                throw;
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
            catch (UnAuthorisedUserExeception ex)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized, ex.Message));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (Exception)
            {

                throw;
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
            catch (UnAuthorisedUserExeception ex)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized, ex.Message));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("user-orders")]
        [Authorize]
        public async Task<ActionResult<List<OrderDto>>> GetUserOrders()
        {
            try
            {
                int userId = ControllerValidator.GetUserIdFromClaims(User.Claims);

                List<OrderDto> orders = await orderService.GetOrdersForUser(userId);

                return Ok(orders);
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
