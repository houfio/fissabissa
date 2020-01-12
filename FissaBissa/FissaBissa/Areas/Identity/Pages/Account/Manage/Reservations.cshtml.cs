using System.Threading.Tasks;
using FissaBissa.Entities;
using FissaBissa.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FissaBissa.Areas.Identity.Pages.Account.Manage
{
    public class ReservationsModel : PageModel
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IReservationRepository _reservationRepo;

        public ReservationsModel(UserManager<UserEntity> userManager, IReservationRepository reservationRepo)
        {
            _userManager = userManager;
            _reservationRepo = reservationRepo;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            ViewData["Reservations"] = await _reservationRepo.Get(null, user.Email);

            return Page();
        }
    }
}
