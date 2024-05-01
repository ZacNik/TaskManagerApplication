using Microsoft.AspNetCore.Identity;

namespace TaskManagerApplication.Models
{
    public class User : IdentityUser
    {
        public string? FullName { get; set; }
        public List<Task>? Tasks { get; set; }
    }

}
