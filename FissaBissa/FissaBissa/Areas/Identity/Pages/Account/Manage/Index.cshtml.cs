using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using FissaBissa.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FissaBissa.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        [BindProperty] public IndexInputModel Input { get; set; }
        [TempData] public string StatusMessage { get; set; }
        public string Username { get; set; }

        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;

        public IndexModel(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        private async Task LoadAsync(UserEntity user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
            Input = new IndexInputModel
            {
                PhoneNumber = phoneNumber
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

        public async Task<IActionResult> OnPostAsync()
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

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);

                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);

                    throw new InvalidOperationException(
                        $"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";

            return RedirectToPage();
        }
    }

    public class IndexInputModel
    {
        [Phone, Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}
