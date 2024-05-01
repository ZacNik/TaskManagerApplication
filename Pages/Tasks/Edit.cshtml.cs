using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagerApplication.Models;

namespace TaskManagerApplication.Pages.Tasks
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _dbContext;
        private readonly ILogger<EditModel> _logger;
        [BindProperty]
        public Models.Task Task { get; set; }

        public EditModel(AppDbContext dbContext, ILogger<EditModel> logger, UserManager<User> userManager)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            _logger.LogInformation("Fetching task with Id: {TaskId}", id);

            Task = await _dbContext.Tasks.FindAsync(id);

            if (Task == null)
            {
                _logger.LogWarning("Task with Id {TaskId} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Task with Id {TaskId} retrieved successfully", id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("Processing form submission for task update.");

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

            // Retrieve the logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogError("Unable to find the logged-in user.");
                ModelState.AddModelError(string.Empty, "Unable to find the logged-in user.");
                return Page();
            }

            _logger.LogInformation("Logged-in user found: {UserId}", user.Id);

            var taskToUpdate = await _dbContext.Tasks.FindAsync(Task.Id);

            if (taskToUpdate == null)
            {
                _logger.LogWarning("Task with Id {TaskId} not found", Task.Id);
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

            _logger.LogInformation("Redirecting to tasks list page after update.");

            return RedirectToPage("./Tasks");
        }
    }
}
