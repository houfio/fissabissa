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
        [BindProperty]
        public IndexInputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public string Username { get; set; }

        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;

        public IndexModel(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        private void Load(UserEntity user)
        {
            Username = user.UserName;
            Input = new IndexInputModel
            {
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Load(user);

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
                Load(user);

                return Page();
            }

            user.FullName = Input.FullName;
            user.PhoneNumber = Input.PhoneNumber;
            user.Address = Input.Address;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";

            return RedirectToPage();
        }
    }

    public class IndexInputModel
    {
        [Required, Display(Name = "Name"), StringLength(100, MinimumLength = 6)]
        public string FullName { get; set; }

        [Required, Phone, Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required, Display(Name = "Address"), StringLength(100, MinimumLength = 6)]
        public string Address { get; set; }
    }
}
