using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskManagerApplication.Models;


namespace TaskManagerApplication.Pages.Tasks
{
    [Authorize]
    public class TasksModel : PageModel
    {
        
        private readonly AppDbContext _dbContext;

        public IEnumerable<Models.Task> Tasks { get; private set; }

        public TasksModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Models.Task>> GetAllTasksAsync()
        {
            return await _dbContext.Tasks.ToListAsync();
        }
        
        public async System.Threading.Tasks.Task OnGetAsync()
        {
            Tasks = await _dbContext.Tasks.ToListAsync(); // Loading tasks from the database
            
        }
    }
}
