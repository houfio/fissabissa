﻿using System;
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
                GetTestAccessories().ForEach(e => database.Add(e));

                return new AccessoryRepository(database);
            });

            // Act
            var response = await repo.Get();

            // Assert
            Assert.Equal(3, response.Count);
        }

        [Fact]
        public async Task Get_Single()
        {
            // Arrange
            var id = Guid.NewGuid();
            var repo = Setup.CreateService(database =>
            {
                GetTestAccessories(id).ForEach(e => database.Add(e));

                return new AccessoryRepository(database);
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
            var repo = Setup.CreateService(database => new AccessoryRepository(database));

            // Act
            var response = await repo.Create(new AccessoryModel
            {
                Name = "test",
                Price = 0
            }, "");

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Id == default);
            Assert.NotNull(response.Animals);
            Assert.NotNull(response.Reservations);
        }

        [Fact]
        public async Task Update()
        {
            // Arrange
            var id = Guid.NewGuid();
            var repo = Setup.CreateService(database =>
            {
                GetTestAccessories(id).ForEach(e => database.Add(e));

                return new AccessoryRepository(database);
            });

            // Act
            await repo.Update(new AccessoryModel
            {
                Id = id,
                Name = "test2",
                Price = 0
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
                GetTestAccessories(id).ForEach(e => database.Add(e));

                return new AccessoryRepository(database);
            });

            // Act
            await repo.Delete(id);
            var response = await repo.Get(id);

            // Assert
            Assert.Null(response);
        }

        private List<AccessoryEntity> GetTestAccessories(Guid? id = null)
        {
            return new List<AccessoryEntity>
            {
                new AccessoryEntity
                {
                    Id = id ?? Guid.NewGuid(),
                    Name = "test",
                    Price = 0,
                    Image = "test",
                    Animals = new List<AnimalAccessoryEntity>(),
                    Reservations = new List<AccessoryReservationEntity>()
                },
                new AccessoryEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "test",
                    Price = 0,
                    Image = "test",
                    Animals = new List<AnimalAccessoryEntity>(),
                    Reservations = new List<AccessoryReservationEntity>()
                },
                new AccessoryEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "test",
                    Price = 0,
                    Image = "test",
                    Animals = new List<AnimalAccessoryEntity>(),
                    Reservations = new List<AccessoryReservationEntity>()
                }
            };
        }
    }
}
