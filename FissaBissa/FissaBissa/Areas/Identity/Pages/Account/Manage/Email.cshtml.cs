using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using FissaBissa.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FissaBissa.Areas.Identity.Pages.Account.Manage
{
    public class EmailModel : PageModel
    {
        [BindProperty]
        public EmailInputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public string Email { get; set; }

        private readonly UserManager<UserEntity> _userManager;

        public EmailModel(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }

        private async Task LoadAsync(UserEntity user)
        {
            Email = await _userManager.GetEmailAsync(user);

            Input = new EmailInputModel
            {
                NewEmail = Email
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);

                return Page();
            }

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
            var changeEmailResult = await _userManager.ChangeEmailAsync(user, Input.NewEmail, token);

            if (!changeEmailResult.Succeeded)
            {
                foreach (var error in changeEmailResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return Page();
            }

            StatusMessage = "Your email has been changed.";

            return RedirectToPage();
        }
    }

    public class EmailInputModel
    {
        [Required, EmailAddress, Display(Name = "New email")]
        public string NewEmail { get; set; }
    }
}
