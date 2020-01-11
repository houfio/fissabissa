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
    public class RegisterModel : PageModel
    {
        [BindProperty] public RegisterInputModel Input { get; set; }
        public string ReturnUrl { get; set; }

        private readonly SignInManager<UserEntity> _signInManager;
        private readonly UserManager<UserEntity> _userManager;

        public RegisterModel(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public void OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = new UserEntity
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    EmailConfirmed = true
                };

                var createResult = await _userManager.CreateAsync(user, Input.Password);

                if (createResult.Succeeded)
                {
                    if (Input.Admin)
                    {
                        var roleResult = await _userManager.AddToRoleAsync(user, "Admin");

                        if (!roleResult.Succeeded)
                        {
                            foreach (var error in roleResult.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }

                            return Page();
                        }
                    }

                    await _signInManager.SignInAsync(user, false);

                    return LocalRedirect(returnUrl);
                }

                foreach (var error in createResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }

    public class RegisterInputModel
    {
        [Required, EmailAddress, Display(Name = "Email")]
        public string Email { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Password"),
         StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
             MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.Password), Display(Name = "Confirm password"),
         Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Administrator")] public bool Admin { get; set; }
    }
}
