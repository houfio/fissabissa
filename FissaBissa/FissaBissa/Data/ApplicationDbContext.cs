using FissaBissa.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FissaBissa.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<AccessoryModel> Accessories { get; set; }
        public DbSet<AnimalModel> Animals { get; set; }
        public DbSet<AnimalTypeModel> AnimalTypes { get; set; }
        public DbSet<ReservationModel> Reservations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AnimalAccessoryModel>()
                .HasKey(t => new {t.AnimalId, t.AccessoryId});

            modelBuilder.Entity<AnimalAccessoryModel>()
                .HasOne(aa => aa.Animal)
                .WithMany(a => a.Accessories)
                .HasForeignKey(aa => aa.AnimalId);

            modelBuilder.Entity<AnimalAccessoryModel>()
                .HasOne(aa => aa.Accessory)
                .WithMany(a => a.Animals)
                .HasForeignKey(a => a.AccessoryId);

            modelBuilder.Entity<AnimalReservationModel>()
                .HasKey(t => new {t.AnimalId, t.ReservationId});

            modelBuilder.Entity<AnimalReservationModel>()
                .HasOne(ar => ar.Animal)
                .WithMany(a => a.Reservations)
                .HasForeignKey(ar => ar.AnimalId);

            modelBuilder.Entity<AnimalReservationModel>()
                .HasOne(ar => ar.Reservation)
                .WithMany(r => r.Animals)
                .HasForeignKey(ar => ar.ReservationId);

            modelBuilder.Entity<AccessoryReservationModel>()
                .HasKey(t => new {t.AccessoryId, t.ReservationId});

            modelBuilder.Entity<AccessoryReservationModel>()
                .HasOne(ar => ar.Accessory)
                .WithMany(a => a.Reservations)
                .HasForeignKey(ar => ar.AccessoryId);

            modelBuilder.Entity<AccessoryReservationModel>()
                .HasOne(ar => ar.Reservation)
                .WithMany(r => r.Accessories)
                .HasForeignKey(ar => ar.ReservationId);
        }
    }
}
