using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TaskManagementAPI.Data;

namespace TaskManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [EnableRateLimiting("AdminPolicy")]
    public class AdminController : ControllerBase
    {
        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = FakeDataStore.Users.Select(u => new
            {
                u.Id,
                u.Username,
                u.Role
            }).ToList();

            return Ok(users);
        }

        [HttpDelete("users/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = FakeDataStore.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            FakeDataStore.Users.Remove(user);

            return Ok(new { message = "User deleted successfully." });
        }
    }
}