using System;
using System.Linq;
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
        private readonly IAnimalRepository _animalRepo;
        private readonly IAccessoryRepository _accessoryRepo;

        public AnimalsController(IAnimalRepository animalRepo, IAccessoryRepository accessoryRepo)
        {
            _animalRepo = animalRepo;
            _accessoryRepo = accessoryRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _animalRepo.Get());
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _animalRepo.Get(id.Value);

            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["TypeId"] = new SelectList(await _animalRepo.GetTypes(), "Id", "Name");
            ViewData["Accessories"] = new MultiSelectList(await _accessoryRepo.Get(), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,TypeId,Price,Image,Accessories")]
            AnimalModel model)
        {
            var path = await FileUtilities.StoreImage(nameof(model.Image), model.Image, ModelState);

            if (ModelState.IsValid)
            {
                await _animalRepo.Create(model, path);

                return RedirectToAction(nameof(Index));
            }

            ViewData["TypeId"] = new SelectList(await _animalRepo.GetTypes(), "Id", "Name", model.TypeId);
            ViewData["Accessories"] = new MultiSelectList(await _accessoryRepo.Get(), "Id", "Name", model.Accessories);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _animalRepo.Get(id.Value);

            if (entity == null)
            {
                return NotFound();
            }

            ViewData["TypeId"] = new SelectList(await _animalRepo.GetTypes(), "Id", "Name", entity.TypeId);
            ViewData["Accessories"] = new MultiSelectList(await _accessoryRepo.Get(), "Id", "Name",
                entity.Accessories.Select((a) => a.AccessoryId).ToList());

            return View(entity.Transform());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid id, [Bind("Id,Name,TypeId,Price,Image,Accessories")]
            AnimalModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var path = await FileUtilities.StoreImage(nameof(model.Image), model.Image, ModelState);

            if (ModelState.IsValid)
            {
                await _animalRepo.Update(model, path);

                return RedirectToAction(nameof(Index));
            }

            ViewData["TypeId"] = new SelectList(await _animalRepo.GetTypes(), "Id", "Name", model.TypeId);
            ViewData["Accessories"] = new MultiSelectList(await _accessoryRepo.Get(), "Id", "Name", model.Accessories);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _animalRepo.Get(id.Value);

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
            await _animalRepo.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
