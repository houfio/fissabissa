using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using FissaBissa.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FissaBissa.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        [BindProperty] public LoginInputModel Input { get; set; }
        [TempData] public string ErrorMessage { get; set; }
        public string ReturnUrl { get; set; }

        private readonly SignInManager<UserEntity> _signInManager;

        public LoginModel(SignInManager<UserEntity> signInManager)
        {
            _signInManager = signInManager;
        }

        public void OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    Input.Email,
                    Input.Password,
                    Input.RememberMe,
                    false
                );

                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return Page();
        }
    }

    public class LoginInputModel
    {
        [Required, EmailAddress] public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")] public bool RememberMe { get; set; }
    }
}
