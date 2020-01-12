using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FissaBissa.Data;
using FissaBissa.Entities;
using FissaBissa.Models;
using FissaBissa.Utilities;
using Microsoft.AspNetCore.Authorization;
using FissaBissa.Repositories;

namespace FissaBissa.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AnimalsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAnimalRepository _AnimalRepo;

        public AnimalsController(ApplicationDbContext context, IAnimalRepository AnimalRepo)
        {
            _context = context;
            _AnimalRepo = AnimalRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _AnimalRepo.Get());
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _AnimalRepo.Get(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["TypeId"] = new SelectList(_context.AnimalTypes, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,TypeId,Price,Image")] AnimalModel model)
        {

            var path = await FileUtilities.StoreImage(nameof(model.Image), model.Image, ModelState);

            if (ModelState.IsValid)
            {

                await Task.Run(() => _AnimalRepo.Create(model, path));

                return RedirectToAction(nameof(Index));
            }

            ViewData["TypeId"] = new SelectList(_context.AnimalTypes, "Id", "Name", model.TypeId);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _AnimalRepo.Get(id);

            if (model == null)
            {
                return NotFound();
            }

            ViewData["TypeId"] = new SelectList(_context.AnimalTypes, "Id", "Name", model.TypeId);

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
                try
                {
                    await Task.Run(() => _AnimalRepo.Update(model, path));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Animals.Any(e => e.Id == model.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["TypeId"] = new SelectList(_context.AnimalTypes, "Id", "Name", model.TypeId);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _AnimalRepo.Get(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await Task.Run(() => _AnimalRepo.DeleteConfirmed(id));

            return RedirectToAction(nameof(Index));
        }
    }
}
