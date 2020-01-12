using System;
using FissaBissa.Models;
using Microsoft.AspNetCore.Mvc;

namespace FissaBissa.Controllers
{
    public class BookingController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([Bind("Date")] ReservationModel model)
        {
            if (DateTime.Today >= model.Date.Date)
            {
                ModelState.AddModelError(nameof(model.Date), "Selected date must be after today");

                return View("../Home/Index", model);
            }

            return View(model);
        }
    }
}
