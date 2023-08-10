using Business.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService=authService;
        }

        [HttpPost("registerPatient")]
        public async Task<IActionResult> RegisterPatientAsync([FromBody]UserRegisterDto userRegisterDto)
        {
            if (userRegisterDto == null) return BadRequest("User data is empty");

            var registerResult = await _authService.RegisterAsync(userRegisterDto);
            if (registerResult==null) return StatusCode(500, "Register user internal server error");
            if (registerResult.InternalServerError) return StatusCode(500, "Register user internal server error. "+registerResult.Message);
            if (!registerResult.IsSuccess) return NotFound(registerResult.Message);
            return Ok(registerResult.Data);
        }

        [HttpPost("loginUser")]
        public async Task<IActionResult> LoginUserAsync(UserLoginDto userLoginDto)
        {
            if (userLoginDto==null) return BadRequest("User data is empty");

            var loginResult = await _authService.LoginAsync(userLoginDto);
            if (loginResult==null) return StatusCode(500, "Login user internal server error");
            if (loginResult.InternalServerError) return StatusCode(500, "Login user internal server error. "+loginResult.Message);
            if (!loginResult.IsSuccess) return Unauthorized(loginResult.Message);
            return Ok(loginResult.Data);
        }
    }
}
