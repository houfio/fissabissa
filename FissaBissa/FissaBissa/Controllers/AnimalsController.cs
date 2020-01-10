using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FissaBissa.Data;
using FissaBissa.Entities;
using FissaBissa.Models;
using FissaBissa.Utilities;

namespace FissaBissa.Controllers
{
    public class AnimalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnimalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Animals.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Animals.FindAsync(id);

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
                var entity = new AnimalEntity();

                entity.Copy(model, true);
                entity.Image = path;
                
                _context.Add(entity);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["TypeId"] = new SelectList(_context.AnimalTypes, "Id", "Name", model.TypeId);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Animals.FindAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            ViewData["TypeId"] = new SelectList(_context.AnimalTypes, "Id", "Name", model.TypeId);

            return View(model.Transform());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, [Bind("Id,Name,TypeId,Price,Image")] AnimalModel model)
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
                    var entity = await _context.Animals.FindAsync(model.Id);

                    entity.Copy(model, false);
                    entity.Image = path;

                    _context.Update(entity);

                    await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Animals.FindAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _context.Animals.FindAsync(id);

            _context.Animals.Remove(model);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
