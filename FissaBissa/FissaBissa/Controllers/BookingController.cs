using System;
using System.Linq;
using System.Threading.Tasks;
using FissaBissa.Entities;
using FissaBissa.Models;
using FissaBissa.Repositories;
using FissaBissa.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FissaBissa.Controllers
{
    public class BookingController : Controller
    {
        private readonly IAnimalRepository _animalRepo;
        private readonly IReservationRepository _reservationRepo;
        private readonly IAccessoryRepository _accessoryRepo;
        private readonly UserManager<UserEntity> _userManager;

        public BookingController(IAnimalRepository animalRepo, IReservationRepository reservationRepo,
            IAccessoryRepository accessoryRepo, UserManager<UserEntity> userManager)
        {
            _animalRepo = animalRepo;
            _reservationRepo = reservationRepo;
            _accessoryRepo = accessoryRepo;
            _userManager = userManager;
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
            ViewData["Unavailable"] = (await _reservationRepo.Get(model.Date))
                .SelectMany((r) => r.Animals)
                .Select((a) => a.AnimalId)
                .ToList();

            return View(nameof(Index), model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accessories([Bind("Date,Animals")] ReservationModel model)
        {
            OrderValidator.ValidateOrder(nameof(model.Animals),
                model.Animals.Select(i => _animalRepo.Get(i).Result).ToList(), model.Date, ModelState);

            if (!ModelState.IsValid)
            {
                return await Index(model);
            }

            ViewData["Accessories"] = await _accessoryRepo.Get();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact([Bind("Date,Animals,Accessories")] ReservationModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                model.FullName ??= user.FullName;
                model.Address ??= user.Address;
                model.Email ??= user.Email;
                model.PhoneNumber ??= user.PhoneNumber;

                ModelState.Clear();
            }

            return View(nameof(Contact), model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm([Bind("Date,Animals,Accessories,FullName,Address,Email,PhoneNumber")]
            ReservationModel model)
        {
            if (!ModelState.IsValid)
            {
                return await Contact(model);
            }

            return View(model);
        }
    }
}
