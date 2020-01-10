using FissaBissa.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FissaBissa.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<AccessoryEntity> Accessories { get; set; }
        public DbSet<AnimalEntity> Animals { get; set; }
        public DbSet<AnimalTypeEntity> AnimalTypes { get; set; }
        public DbSet<ReservationEntity> Reservations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AnimalAccessoryEntity>()
                .HasKey(t => new {t.AnimalId, t.AccessoryId});

            modelBuilder.Entity<AnimalAccessoryEntity>()
                .HasOne(aa => aa.Animal)
                .WithMany(a => a.Accessories)
                .HasForeignKey(aa => aa.AnimalId);

            modelBuilder.Entity<AnimalAccessoryEntity>()
                .HasOne(aa => aa.Accessory)
                .WithMany(a => a.Animals)
                .HasForeignKey(a => a.AccessoryId);

            modelBuilder.Entity<AnimalReservationEntity>()
                .HasKey(t => new {t.AnimalId, t.ReservationId});

            modelBuilder.Entity<AnimalReservationEntity>()
                .HasOne(ar => ar.Animal)
                .WithMany(a => a.Reservations)
                .HasForeignKey(ar => ar.AnimalId);

            modelBuilder.Entity<AnimalReservationEntity>()
                .HasOne(ar => ar.Reservation)
                .WithMany(r => r.Animals)
                .HasForeignKey(ar => ar.ReservationId);

            modelBuilder.Entity<AccessoryReservationEntity>()
                .HasKey(t => new {t.AccessoryId, t.ReservationId});

            modelBuilder.Entity<AccessoryReservationEntity>()
                .HasOne(ar => ar.Accessory)
                .WithMany(a => a.Reservations)
                .HasForeignKey(ar => ar.AccessoryId);

            modelBuilder.Entity<AccessoryReservationEntity>()
                .HasOne(ar => ar.Reservation)
                .WithMany(r => r.Accessories)
                .HasForeignKey(ar => ar.ReservationId);
        }
    }
}
