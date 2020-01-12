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
    public class AnimalRepositoryTests
    {
        [Fact]
        public async Task Get_All()
        {
            // Arrange
            var repo = Setup.CreateService(database =>
            {
                GetTestAnimals().ForEach(e => database.Add(e));

                return new AnimalRepository(database);
            });

            // Act
            var response = await repo.Get();

            // Assert
            Assert.Equal(2, response.Count);
        }

        [Fact]
        public async Task GetTypes()
        {
            // Arrange
            var repo = Setup.CreateService(database =>
            {
                GetTestTypes().ForEach(e => database.Add(e));

                return new AnimalRepository(database);
            });

            // Act
            var response = await repo.GetTypes();

            // Assert
            Assert.Equal(1, response.Count);
        }

        [Fact]
        public async Task Get_Single()
        {
            // Arrange
            var id = Guid.NewGuid();
            var repo = Setup.CreateService(database =>
            {
                GetTestAnimals(id).ForEach(e => database.Add(e));

                return new AnimalRepository(database);
            });

            // Act
            var response = await repo.Get(id);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(id, response.Id);
        }

        [Fact]
        public async Task Create()
        {
            // Arrange
            var id = Guid.NewGuid();
            var repo = Setup.CreateService(database =>
            {
                GetTestTypes(id).ForEach(e => database.Add(e));

                return new AnimalRepository(database);
            });

            // Act
            var response = await repo.Create(new AnimalModel
            {
                Name = "test",
                TypeId = id,
                Price = 0,
                Accessories = new List<Guid>()
            }, "");

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Id == default);
            Assert.NotNull(response.Accessories);
            Assert.NotNull(response.Reservations);
        }

        [Fact]
        public async Task Update()
        {
            // Arrange
            var id = Guid.NewGuid();
            var repo = Setup.CreateService(database =>
            {
                GetTestAnimals(id).ForEach(e => database.Add(e));

                return new AnimalRepository(database);
            });

            // Act
            await repo.Update(new AnimalModel
            {
                Id = id,
                Name = "test2",
                TypeId = id,
                Price = 0,
                Accessories = new List<Guid>()
            }, "");
            var response = await repo.Get(id);

            // Assert
            Assert.Equal("test2", response.Name);
        }

        [Fact]
        public async Task Delete()
        {
            // Arrange
            var id = Guid.NewGuid();
            var repo = Setup.CreateService(database =>
            {
                GetTestAnimals(id).ForEach(e => database.Add(e));

                return new AnimalRepository(database);
            });

            // Act
            await repo.Delete(id);
            var response = await repo.Get(id);

            // Assert
            Assert.Null(response);
        }

        private List<AnimalEntity> GetTestAnimals(Guid? id = null)
        {
            return new List<AnimalEntity>
            {
                new AnimalEntity
                {
                    Id = id ?? Guid.NewGuid(),
                    Name = "test",
                    TypeId = Guid.NewGuid(),
                    Price = 0,
                    Image = "test",
                    Accessories = new List<AnimalAccessoryEntity>(),
                    Reservations = new List<AnimalReservationEntity>()
                },
                new AnimalEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "test",
                    TypeId = Guid.NewGuid(),
                    Price = 0,
                    Image = "test",
                    Accessories = new List<AnimalAccessoryEntity>(),
                    Reservations = new List<AnimalReservationEntity>()
                }
            };
        }

        private List<AnimalTypeEntity> GetTestTypes(Guid? id = null)
        {
            return new List<AnimalTypeEntity>
            {
                new AnimalTypeEntity
                {
                    Id = id ?? Guid.NewGuid(),
                    Name = "test"
                }
            };
        }
    }
}
