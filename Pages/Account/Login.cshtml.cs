using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using TaskManagerApplication.Models;
using Microsoft.AspNetCore.Authorization;

namespace TaskManagerApplication.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;

        public LoginModel(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, false, false);

            if (result.Succeeded)
            {
                return RedirectToPage("/Tasks/Tasks");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
        }
    }

    public class LoginInputModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
