using AutoMapper;
using EduQuest.Commons;
using EduQuest.Features.Auth.DTOS;
using EduQuest.Features.Users;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest.Features.Auth
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService, IMapper mapper, ITokenService tokenService) : Controller
    {
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] AuthRequestDto request)
        {
            try
            {
                var user = await authService.Login(request);
                var response = mapper.Map<AuthResponseDto>(user);
                response.Token = tokenService.GenerateUserToken(user);
                return Ok(response);
            }
            catch (UserNotFoundException ex)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized, "Invalid Credentials"));
            }
            catch (InvalideCredsException ex)
            {
                return Unauthorized(new ErrorModel(StatusCodes.Status401Unauthorized, ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto request)
        {
            try
            {
                var user = await authService.Register(request);

                var response = mapper.Map<AuthResponseDto>(user);
                response.Token = tokenService.GenerateUserToken(user);
                return Ok(response);

            }
            catch (UserAlreadyExistsException ex)
            {
                return BadRequest(new ErrorModel(StatusCodes.Status400BadRequest, ex.Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500);
            }
        }
    }
}
