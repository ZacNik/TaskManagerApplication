using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagerApplication.Models;

namespace TaskManagerApplication.Pages.Tasks
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly AppDbContext _dbContext;

        [BindProperty]
        public TaskManagerApplication.Models.Task Task { get; set; }

        public EditModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Task = await _dbContext.Tasks.FindAsync(id);

            if (Task == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var taskToUpdate = await _dbContext.Tasks.FindAsync(Task.Id);

            if (taskToUpdate == null)
            {
                return NotFound();
            }

            taskToUpdate.Title = Task.Title;
            taskToUpdate.Description = Task.Description;
            taskToUpdate.Priority = Task.Priority;
            taskToUpdate.DueDate = Task.DueDate;

            await _dbContext.SaveChangesAsync();

            return RedirectToPage("./Tasks");
        }
    }
}
