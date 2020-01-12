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
    public interface IReservationRepository
    {
        Task<ICollection<ReservationEntity>> Get(DateTime? date = null);
        Task<ReservationEntity> Get(Guid id);
        Task Create(ReservationModel model);
        Task Delete(Guid id);
    }

    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<ReservationEntity>> Get(DateTime? date)
        {
            return await _context.Reservations
                .Where((r) => date == null || r.Date.Date == date.Value.Date)
                .ToListAsync();
        }

        public async Task<ReservationEntity> Get(Guid id)
        {
            return await _context.Reservations.FindAsync(id);
        }

        public async Task Create(ReservationModel model)
        {
            var entity = new ReservationEntity();

            entity.Copy(model, true);

            _context.Add(entity);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            _context.Remove(await Get(id));

            await _context.SaveChangesAsync();
        }
    }
}
