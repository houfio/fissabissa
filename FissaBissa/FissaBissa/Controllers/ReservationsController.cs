using System;
using System.Threading.Tasks;
using FissaBissa.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FissaBissa.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReservationsController : Controller
    {
        private readonly IReservationRepository _reservationRepo;

        public ReservationsController(IReservationRepository reservationRepo)
        {
            _reservationRepo = reservationRepo;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _reservationRepo.Get());
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _reservationRepo.Get(id.Value);

            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _reservationRepo.Get(id.Value);

            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _reservationRepo.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
