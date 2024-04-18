using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagerApplication.Models;

namespace TaskManagerApplication.Pages.Tasks
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _dbContext;

        [BindProperty]
        public Models.Task Task { get; set; }

        public CreateModel(AppDbContext dbContext, UserManager<User> userManager)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public void OnGet()
        {
        }


        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                Task.UserId = user.Id;
            }
            
            // Add the task to the database
            _dbContext.Tasks.Add(Task);
            await _dbContext.SaveChangesAsync();

            // Redirect to the tasks list page after creation
            return RedirectToPage("./Tasks");
        }
    }
}
