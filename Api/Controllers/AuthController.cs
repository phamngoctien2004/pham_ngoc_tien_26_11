using Application.DTOs.Common;
using Application.DTOs.Request.Auth;
using Application.DTOs.Response.Auth;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var authResponse = await _authService.Login(loginRequest);
            return Ok(BaseResponseDTO<AuthResponse>.SuccessResponse(authResponse, null, "Login successfully"));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            var authResponse = await _authService.Register(registerRequest);
            return Ok(BaseResponseDTO<AuthResponse>.SuccessResponse(authResponse, null, "Register successfully"));
        }
    }
}
