using FissaBissa.Data;
using FissaBissa.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FissaBissa.Repositories
{
    public interface IAccessoryRepository
    {
        Task<ICollection<AccessoryEntity>> Get();
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
    }
}
