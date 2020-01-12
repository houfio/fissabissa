using FissaBissa.Data;
using FissaBissa.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FissaBissa.Repositories
{

    public interface IAnimalTypeRepository
    {
        Task<ICollection<AnimalTypeEntity>> Get();
    }

   

    public class AnimalTypeRepository : IAnimalTypeRepository
    {

        private readonly ApplicationDbContext _context;

        public AnimalTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<AnimalTypeEntity>> Get()
        {
            return await _context.AnimalTypes.ToListAsync();
        }


    }
}
