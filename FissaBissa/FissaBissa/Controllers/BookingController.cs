using System;
using System.Linq;
using System.Threading.Tasks;
using FissaBissa.Entities;
using FissaBissa.Models;
using FissaBissa.Repositories;
using FissaBissa.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ServiceReference;

namespace FissaBissa.Controllers
{
    public class BookingController : Controller
    {
        private readonly IAnimalRepository _animalRepo;
        private readonly IReservationRepository _reservationRepo;
        private readonly IAccessoryRepository _accessoryRepo;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IService _service;

        public BookingController(IAnimalRepository animalRepo, IReservationRepository reservationRepo,
            IAccessoryRepository accessoryRepo, UserManager<UserEntity> userManager, IService service)
        {
            _animalRepo = animalRepo;
            _reservationRepo = reservationRepo;
            _accessoryRepo = accessoryRepo;
            _userManager = userManager;
            _service = service;
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

            if (ModelState.GetFieldValidationState(nameof(model.Animals)) != ModelValidationState.Valid)
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

            var animals = model.Animals
                .Select((a) => _animalRepo.Get(a).Result)
                .ToList();
            var accessories = model.Accessories
                .Select(a => _accessoryRepo.Get(a).Result)
                .ToList();
            var discounts = await _service.GetDiscountAsync(new DataModel
            {
                Date = model.Date,
                Animals = animals.Select((a) => new ServiceReference.AnimalModel
                    {
                        Name = a.Name,
                        Type = a.Type.Name
                    })
                    .ToArray()
            });
            var discount = Math.Min(60, discounts.Values.Sum());
            var price = animals.Select(a => a.Price).Sum() + accessories.Select(a => a.Price).Sum();

            ViewData["Animals"] = animals.ToDictionary(a => a.Name, a => a.Price.ToString("F"));
            ViewData["Accessories"] = accessories.ToDictionary(a => a.Name, a => a.Price.ToString("F"));
            ViewData["Discounts"] = discounts;
            ViewData["TotalPrice"] = (price * (1.0 - discount / 100.0)).ToString("F");

            model.Price = ViewData["TotalPrice"] as string;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Finish(
            [Bind("Date,Animals,Accessories,FullName,Address,Email,PhoneNumber,Price")]
            ReservationModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            var entity = await _reservationRepo.Create(model);

            return RedirectToAction("Details", "Reservations", new
            {
                id = entity.Id
            });
        }
    }
}
