using System.Linq;
using System.Threading.Tasks;
using FissaBissa.Data;
using FissaBissa.Entities;
using FissaBissa.Models;
using FissaBissa.Repositories;
using FissaBissa.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FissaBissa.Controllers
{
    public class AccessoriesController : Controller
    {

        private readonly ApplicationDbContext _context;

        public AccessoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Accessories.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Accessories.FindAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
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
                var entity = new AccessoryEntity();

                entity.Copy(model, true);
                entity.Image = path;

                _context.Add(entity);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Accessories.FindAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model.Transform());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, [Bind("Id,Name,TypeId,Price,Image")] AccessoryModel model)
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
                    var entity = await _context.Accessories.FindAsync(model.Id);

                    entity.Copy(model, false);
                    entity.Image = path;

                    _context.Update(entity);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Accessories.Any(e => e.Id == model.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Accessories.FindAsync(id);

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
            var model = await _context.Accessories.FindAsync(id);

            _context.Accessories.Remove(model);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}