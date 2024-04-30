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
        private readonly ILogger<EditModel> _logger;
        [BindProperty]
        public TaskManagerApplication.Models.Task Task { get; set; }

        public EditModel(AppDbContext dbContext, ILogger<EditModel> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
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
                _logger.LogWarning("Form data is invalid.");
                // Log model state errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogError("ModelState error: {ErrorMessage}", error.ErrorMessage);
                }
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

            try
            {
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Task updated successfully with Id: {TaskId}", Task.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the task.");
                // Log or handle the exception as necessary
                ModelState.AddModelError(string.Empty, "An error occurred while saving the task.");
                return Page();
            }

            return RedirectToPage("./Tasks");
        }
    }
}
