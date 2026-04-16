using TaskManagementAPI.Models;

namespace TaskManagementAPI.Data
{
    public static class FakeDataStore
    {
        public static List<UserModel> Users = new List<UserModel>
        {
            new UserModel { Id = 1, Username = "admin", Password = "admin123", Role = "Admin" },
            new UserModel { Id = 2, Username = "manager", Password = "manager123", Role = "Manager" },
            new UserModel { Id = 3, Username = "employee", Password = "employee123", Role = "Employee" }
        };

        public static List<TaskItem> Tasks = new List<TaskItem>
        {
            new TaskItem { Id = 1, Title = "Prepare report", Description = "Prepare monthly performance report", Status = "Pending" },
            new TaskItem { Id = 2, Title = "Fix bug", Description = "Resolve login issue", Status = "In Progress" }
        };
    }
}