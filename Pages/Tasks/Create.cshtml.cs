using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagerApplication.Models;
using Microsoft.Extensions.Logging;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace TaskManagerApplication.Pages.Tasks
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _dbContext;
        private readonly ILogger<CreateModel> _logger;

        [BindProperty]
        public Models.Task Task { get; set; }

        public CreateModel(AppDbContext dbContext, UserManager<User> userManager, ILogger<CreateModel>logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        public void OnGet()
        {
            // No specific logic needed for GET request
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
                // Return the form with validation messages if the form data is invalid
                return Page();
            }

            // Retrieve the logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogError("Unable to find the logged-in user.");
                // If the user is not found, return an error (this should generally not happen if the user is authenticated)
                ModelState.AddModelError(string.Empty, "Unable to find the logged-in user.");
                return Page();
            }

            // Log the task details
            _logger.LogInformation("Creating a new task with Title: {Title}, Description: {Description}, Priority: {Priority}, DueDate: {DueDate}, UserId: {UserId}",
                                   Task.Title, Task.Description, Task.Priority, Task.DueDate, user.Id);

            // Set the task's UserId to the logged-in user's ID
            Task.UserId = user.Id;

            // Add the task to the database
            _dbContext.Tasks.Add(Task);

            // Try to save changes to the database
            try
            {
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Task created successfully with Id: {TaskId}", Task.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the task.");
                // Log or handle the exception as necessary
                ModelState.AddModelError(string.Empty, "An error occurred while saving the task.");
                return Page();
            }

            // Redirect to the tasks list page after creation
            return RedirectToPage("./Tasks");
        }
    }
}
