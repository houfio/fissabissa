using System;
using System.Collections.Generic;
using System.Linq;
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
        Task<AnimalEntity> Create(AnimalModel model, string path);
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

        public async Task<AnimalEntity> Create(AnimalModel model, string path)
        {
            var entity = new AnimalEntity();

            entity.Copy(model, true);
            entity.Image = path;
            
            UpdateAccessories(entity, model.Accessories);
            _context.Add(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task Update(AnimalModel model, string path)
        {
            var entity = await Get(model.Id);

            entity.Copy(model, false);
            entity.Image = path;
            
            UpdateAccessories(entity, model.Accessories);
            _context.Update(entity);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            _context.Remove(await Get(id));

            await _context.SaveChangesAsync();
        }

        private void UpdateAccessories(AnimalEntity entity, ICollection<Guid> accessories)
        {
            if (accessories == null)
            {
                accessories = new List<Guid>();
            }

            entity.Accessories
                .Where((c) => !accessories.Contains(c.AccessoryId))
                .ToList()
                .ForEach((c) => entity.Accessories.Remove(c));

            accessories
                .Where((c) => entity.Accessories.All(a => a.AccessoryId != c))
                .ToList()
                .ForEach((c) => entity.Accessories.Add(new AnimalAccessoryEntity
                {
                    Animal = entity,
                    AccessoryId = c
                }));
        }
    }
}
