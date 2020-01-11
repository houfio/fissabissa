using System.Collections.Generic;
using System.Threading.Tasks;
using FissaBissa.Data;
using FissaBissa.Entities;
using Microsoft.EntityFrameworkCore;

namespace FissaBissa.Repositories
{
    public interface IAnimalRepository
    {
        Task<ICollection<AnimalEntity>> Get();
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
    }
}
