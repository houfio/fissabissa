using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FissaBissa.Data;
using FissaBissa.Entities;
using FissaBissa.Models;
using Microsoft.EntityFrameworkCore;

namespace FissaBissa.Repositories
{
    public interface IAnimalRepository
    {
        Task<ICollection<AnimalEntity>> Get();
        Task<ICollection<AnimalTypeEntity>> GetTypes();
        Task<AnimalEntity> Get(Guid id);
        Task Create(AnimalModel model, string path);
        Task Update(AnimalModel model, string path);
        Task Delete(Guid id);
    }

    public class AnimalRepository : IAnimalRepository
    {
        private readonly ApplicationDbContext _context;

        public AnimalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<AnimalEntity>> Get()
        {
            return await _context.Animals.ToListAsync();
        }

        public async Task<ICollection<AnimalTypeEntity>> GetTypes()
        {
            return await _context.AnimalTypes.ToListAsync();
        }

        public async Task<AnimalEntity> Get(Guid id)
        {
            return await _context.Animals.FindAsync(id);
        }

        public async Task Create(AnimalModel model, string path)
        {
            var entity = new AnimalEntity();

            entity.Copy(model, true);
            entity.Image = path;

            _context.Add(entity);

            await _context.SaveChangesAsync();
        }

        public async Task Update(AnimalModel model, string path)
        {
            var entity = await _context.Animals.FindAsync(model.Id);

            entity.Copy(model, false);
            entity.Image = path;

            _context.Update(entity);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var model = await _context.Animals.FindAsync(id);

            _context.Animals.Remove(model);

            await _context.SaveChangesAsync();
        }
    }
}
