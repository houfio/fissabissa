﻿using System;
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
        Task<ICollection<ReservationEntity>> Get(DateTime? date = null, string email = null);
        Task<ReservationEntity> Get(Guid id);
        Task<ReservationEntity> Create(ReservationModel model);
        Task Delete(Guid id);
    }

    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<ReservationEntity>> Get(DateTime? date, string email)
        {
            return await _context.Reservations
                .Where(r => date == null || r.Date.Date == date.Value.Date)
                .Where(r => email == null || r.Email == email)
                .ToListAsync();
        }

        public async Task<ReservationEntity> Get(Guid id)
        {
            return await _context.Reservations.FindAsync(id);
        }

        public async Task<ReservationEntity> Create(ReservationModel model)
        {
            var entity = new ReservationEntity();

            entity.Copy(model, true);

            model.Animals.ToList().ForEach((a) => entity.Animals.Add(new AnimalReservationEntity
            {
                Reservation = entity,
                AnimalId = a
            }));

            model.Accessories.ToList().ForEach((a) => entity.Accessories.Add(new AccessoryReservationEntity
            {
                Reservation = entity,
                AccessoryId = a
            }));

            _context.Add(entity);

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
