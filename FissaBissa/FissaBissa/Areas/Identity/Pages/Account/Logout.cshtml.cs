using System.Threading.Tasks;
using FissaBissa.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FissaBissa.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<UserEntity> _signInManager;

        public LogoutModel(SignInManager<UserEntity> signInManager)
        {
            _signInManager = signInManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToPage();
        }
    }
}
