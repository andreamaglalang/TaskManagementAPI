using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Employee")]
        [EnableRateLimiting("TasksGetPolicy")]
        public IActionResult GetTasks()
        {
            return Ok(FakeDataStore.Tasks);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [EnableRateLimiting("TasksWritePolicy")]
        public IActionResult CreateTask([FromBody] TaskItem task)
        {
            task.Id = FakeDataStore.Tasks.Any() ? FakeDataStore.Tasks.Max(t => t.Id) + 1 : 1;
            FakeDataStore.Tasks.Add(task);

            return Ok(new
            {
                message = "Task created successfully.",
                task
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        [EnableRateLimiting("TasksWritePolicy")]
        public IActionResult UpdateTask(int id, [FromBody] TaskItem updatedTask)
        {
            var task = FakeDataStore.Tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                return NotFound(new { message = "Task not found." });
            }

            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.Status = updatedTask.Status;

            return Ok(new
            {
                message = "Task updated successfully.",
                task
            });
        }
    }
}