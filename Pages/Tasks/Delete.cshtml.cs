using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagerApplication.Models;

namespace TaskManagerApplication.Pages.Tasks
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _dbContext;

        [BindProperty]
        public TaskManagerApplication.Models.Task Task { get; set; }

        public DeleteModel(AppDbContext dbContext)
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

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Task = await _dbContext.Tasks.FindAsync(id);

            if (Task != null)
            {
                _dbContext.Tasks.Remove(Task);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToPage("./Tasks");
        }
    }
}
