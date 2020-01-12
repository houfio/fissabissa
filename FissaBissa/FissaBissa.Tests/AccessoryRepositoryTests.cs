using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FissaBissa.Entities;
using FissaBissa.Models;
using FissaBissa.Repositories;
using FissaBissa.Tests.Utilities;
using Xunit;

namespace FissaBissa.Tests
{
    public class AccessoryRepositoryTests
    {
        [Fact]
        public async Task Get_All()
        {
            // Arrange
            var repo = Setup.CreateService(database =>
            {
                GetTestReservations().ForEach(e => database.Add(e));

                return new ReservationRepository(database);
            });

            // Act
            var response = await repo.Get(null, null);

            // Assert
            Assert.Equal(3, response.Count);
        }

        [Fact]
        public async Task Get_DateFilter()
        {
            // Arrange
            var repo = Setup.CreateService(database =>
            {
                GetTestReservations().ForEach(e => database.Add(e));

                return new ReservationRepository(database);
            });

            // Act
            var response = await repo.Get(DateTime.Now, null);

            // Assert
            Assert.Equal(2, response.Count);
        }

        [Fact]
        public async Task Get_UserFilter()
        {
            // Arrange
            var repo = Setup.CreateService(database =>
            {
                GetTestReservations().ForEach(e => database.Add(e));

                return new ReservationRepository(database);
            });

            // Act
            var response = await repo.Get(null, "test@test.com");

            // Assert
            Assert.Equal(2, response.Count);
        }

        [Fact]
        public async Task Get_Single()
        {
            // Arrange
            var id = Guid.NewGuid();
            var repo = Setup.CreateService(database =>
            {
                GetTestReservations(id).ForEach(e => database.Add(e));

                return new ReservationRepository(database);
            });

            // Act
            var response = await repo.Get(id);

            // Assert
            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_SingleUnknown()
        {
            // Arrange
            var repo = Setup.CreateService(database =>
            {
                GetTestReservations().ForEach(e => database.Add(e));

                return new ReservationRepository(database);
            });

            // Act
            var response = await repo.Get(Guid.NewGuid());

            // Assert
            Assert.Null(response);
        }

        [Fact]
        public async Task Create()
        {
            // Arrange
            var repo = Setup.CreateService(database =>
            {
                GetTestReservations().ForEach(e => database.Add(e));

                return new ReservationRepository(database);
            });

            // Act
            var response = await repo.Create(new ReservationModel
            {
                Date = DateTime.Today,
                Animals = new List<Guid>(),
                Accessories = new List<Guid>(),
                FullName = "test",
                Address = "test",
                Email = "test",
                PhoneNumber = "test",
                Price = "0.00"
            });

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Id == default);
            Assert.NotNull(response.Animals);
            Assert.NotNull(response.Accessories);
        }

        [Fact]
        public async Task Delete()
        {
            // Arrange
            var id = Guid.NewGuid();
            var repo = Setup.CreateService(database =>
            {
                GetTestReservations(id).ForEach(e => database.Add(e));

                return new ReservationRepository(database);
            });

            // Act
            await repo.Delete(id);
            var response = await repo.Get(null, null);

            // Assert
            Assert.Equal(2, response.Count);
        }

        private List<ReservationEntity> GetTestReservations(Guid? id = null)
        {
            return new List<ReservationEntity>
            {
                new ReservationEntity
                {
                    Id = id ?? Guid.NewGuid(),
                    Date = DateTime.Today,
                    Name = "test",
                    Address = "Teststreet",
                    Email = "test@test.com",
                    Telephone = "5345345345",
                    Price = "0.00",
                    Animals = new List<AnimalReservationEntity>(),
                    Accessories = new List<AccessoryReservationEntity>()
                },
                new ReservationEntity
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.Today,
                    Name = "test",
                    Address = "Teststreet",
                    Email = "",
                    Telephone = "",
                    Price = "0.00",
                    Animals = new List<AnimalReservationEntity>(),
                    Accessories = new List<AccessoryReservationEntity>()
                },
                new ReservationEntity
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.Today.AddDays(1),
                    Name = "test",
                    Address = "Teststreet",
                    Email = "test@test.com",
                    Telephone = "5345345345",
                    Price = "0.00",
                    Animals = new List<AnimalReservationEntity>(),
                    Accessories = new List<AccessoryReservationEntity>()
                }
            };
        }
    }
}
