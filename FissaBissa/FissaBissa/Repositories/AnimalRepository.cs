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
        Task<AnimalEntity> Get(Guid? id);
        void Create(AnimalModel model, string path);
        Task<AnimalEntity> Update(Guid? id);
        void Update(AnimalModel model, string path);
        Task<AnimalEntity> Delete(Guid? id);
        void DeleteConfirmed(Guid id);
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

        public async Task<AnimalEntity> Get(Guid? id)
        {
            return await _context.Animals.FindAsync(id);
        }

        public async void Create(AnimalModel model, string path)
        {
            var entity = new AnimalEntity();

            entity.Copy(model, true);
            entity.Image = path;

            _context.Add(entity);

           await _context.SaveChangesAsync();
        }

        public async Task<AnimalEntity> Update(Guid? id)
        {
            return await _context.Animals.FindAsync(id);
        }

        public async void Update(AnimalModel model, string path)
        {
            var entity = await _context.Animals.FindAsync(model.Id);

            entity.Copy(model, false);
            entity.Image = path;

            _context.Update(entity);

            await _context.SaveChangesAsync();
        }

        public async Task<AnimalEntity> Delete(Guid? id)
        {
            return await _context.Animals.FindAsync(id);
        }

        public async void DeleteConfirmed(Guid id)
        {
            var model = await _context.Animals.FindAsync(id);

            _context.Animals.Remove(model);

            await _context.SaveChangesAsync();
        }
    }
}
