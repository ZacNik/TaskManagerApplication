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
        public string SortOrder { get; set; }
        public string CurrentFilter { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;



        public TasksModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Models.Task>> GetAllTasksAsync()
        {
            return await _dbContext.Tasks.ToListAsync();
        }
        
        public async System.Threading.Tasks.Task OnGetAsync(string sortOrder, string currentFilter, int pageNumber)
        {
            SortOrder = sortOrder ?? "";
            CurrentFilter = currentFilter ?? "";
            PageNumber = pageNumber > 0 ? pageNumber : 1;
            IQueryable<Models.Task> taskQuery = _dbContext.Tasks;

            // Filtering
            if (!string.IsNullOrEmpty(CurrentFilter))
            {
                taskQuery = taskQuery.Where(t => t.Title.Contains(CurrentFilter));
            }

            // Apply sorting
            switch (SortOrder)
            {
                case "title_desc":
                    taskQuery = taskQuery.OrderByDescending(t => t.Title);
                    break;
                case "priority":
                    taskQuery = taskQuery.OrderBy(t => t.Priority);
                    break;
                case "priority_desc":
                    taskQuery = taskQuery.OrderByDescending(t => t.Priority);
                    break;
                default:
                    taskQuery = taskQuery.OrderBy(t => t.Title);
                    break;
            }

            // Apply pagination
            Tasks = await taskQuery
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            Tasks = await _dbContext.Tasks.ToListAsync(); // Loading tasks from the database
            
        }
    }
}
