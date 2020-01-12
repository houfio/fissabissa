using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FissaBissa.Models;
using FissaBissa.Utilities;
using Microsoft.AspNetCore.Authorization;
using FissaBissa.Repositories;

namespace FissaBissa.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AnimalsController : Controller
    {
        private readonly IAnimalRepository _repo;

        public AnimalsController(IAnimalRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _repo.Get());
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _repo.Get(id.Value);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["TypeId"] = new SelectList(await _repo.GetTypes(), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,TypeId,Price,Image")] AnimalModel model)
        {
            var path = await FileUtilities.StoreImage(nameof(model.Image), model.Image, ModelState);

            if (ModelState.IsValid)
            {
                await _repo.Create(model, path);

                return RedirectToAction(nameof(Index));
            }

            ViewData["TypeId"] = new SelectList(await _repo.GetTypes(), "Id", "Name", model.TypeId);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _repo.Get(id.Value);

            if (model == null)
            {
                return NotFound();
            }

            ViewData["TypeId"] = new SelectList(await _repo.GetTypes(), "Id", "Name", model.TypeId);

            return View(model.Transform());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid id, [Bind("Id,Name,TypeId,Price,Image")] AnimalModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var path = await FileUtilities.StoreImage(nameof(model.Image), model.Image, ModelState);

            if (ModelState.IsValid)
            {
                await _repo.Update(model, path);

                return RedirectToAction(nameof(Index));
            }

            ViewData["TypeId"] = new SelectList(await _repo.GetTypes(), "Id", "Name", model.TypeId);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _repo.Get(id.Value);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _repo.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
