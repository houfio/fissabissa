﻿using Microsoft.AspNetCore.Mvc;

namespace FissaBissa.Controllers
{
    public class ReservationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
