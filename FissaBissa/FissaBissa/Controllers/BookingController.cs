using System;
using System.Threading.Tasks;
using FissaBissa.Models;
using FissaBissa.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FissaBissa.Controllers
{
    public class BookingController : Controller
    {
        private readonly IAnimalRepository _animalRepo;

        public BookingController(IAnimalRepository animalRepo)
        {
            _animalRepo = animalRepo;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Date")] ReservationModel model)
        {
            if (DateTime.Today >= model.Date.Date)
            {
                ModelState.AddModelError(nameof(model.Date), "Selected date must be after today");

                return View("../Home/Index", model);
            }

            ViewData["Animals"] = await _animalRepo.Get();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accessories([Bind("Date,Animals")] ReservationModel model)
        {
            return View(model);
        }
    }
}
