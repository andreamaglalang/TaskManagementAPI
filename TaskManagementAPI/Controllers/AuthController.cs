using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;
using TaskManagementAPI.Utils;

namespace TaskManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        [ApiKeyAuthorize]
        [EnableRateLimiting("LoginPolicy")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            var user = FakeDataStore.Users.FirstOrDefault(u =>
                u.Username == loginModel.Username &&
                u.Password == loginModel.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            var jwtKey = _configuration["Jwt:Key"];

            if (string.IsNullOrEmpty(jwtKey))
            {
                return StatusCode(500, new { message = "JWT key is missing." });
            }

            var jwtService = new JwtService(jwtKey);
            var token = jwtService.GenerateToken(user.Username, user.Role);

            return Ok(new
            {
                message = "Login successful.",
                username = user.Username,
                role = user.Role,
                token = token
            });
        }
    }
}