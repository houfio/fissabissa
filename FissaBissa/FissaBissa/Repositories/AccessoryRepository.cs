using System;
using FissaBissa.Data;
using FissaBissa.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using FissaBissa.Models;

namespace FissaBissa.Repositories
{
    public interface IAccessoryRepository
    {
        Task<ICollection<AccessoryEntity>> Get();
        Task<AccessoryEntity> Get(Guid id);
        Task<AccessoryEntity> Create(AccessoryModel model, string path);
        Task<AccessoryEntity> Update(AccessoryModel model, string path);
        Task Delete(Guid id);
    }

    public class AccessoryRepository : IAccessoryRepository
    {
        private readonly ApplicationDbContext _context;

        public AccessoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<AccessoryEntity>> Get()
        {
            return await _context.Accessories.ToListAsync();
        }

        public async Task<AccessoryEntity> Get(Guid id)
        {
            return await _context.Accessories.FindAsync(id);
        }

        public async Task<AccessoryEntity> Create(AccessoryModel model, string path)
        {
            var entity = new AccessoryEntity();

            entity.Copy(model, true);
            entity.Image = path;

            _context.Add(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<AccessoryEntity> Update(AccessoryModel model, string path)
        {
            var entity = await Get(model.Id);

            entity.Copy(model, false);
            entity.Image = path;

            _context.Update(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task Delete(Guid id)
        {
            _context.Remove(await Get(id));

            await _context.SaveChangesAsync();
        }
    }
}
