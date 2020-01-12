using System;
using System.Threading.Tasks;
using FissaBissa.Models;
using FissaBissa.Repositories;
using FissaBissa.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FissaBissa.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccessoriesController : Controller
    {
        private readonly IAccessoryRepository _accessoryRepo;

        public AccessoriesController(IAccessoryRepository accessoryRepo)
        {
            _accessoryRepo = accessoryRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _accessoryRepo.Get());
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _accessoryRepo.Get(id.Value);

            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Image")] AccessoryModel model)
        {
            var path = await FileUtilities.StoreImage(nameof(model.Image), model.Image, ModelState);

            if (ModelState.IsValid)
            {
                await _accessoryRepo.Create(model, path);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _accessoryRepo.Get(id.Value);

            if (entity == null)
            {
                return NotFound();
            }

            return View(entity.Transform());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid id, [Bind("Id,Name,TypeId,Price,Image")] AccessoryModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var path = await FileUtilities.StoreImage(nameof(model.Image), model.Image, ModelState);

            if (ModelState.IsValid)
            {
                await _accessoryRepo.Update(model, path);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _accessoryRepo.Get(id.Value);

            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _accessoryRepo.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
