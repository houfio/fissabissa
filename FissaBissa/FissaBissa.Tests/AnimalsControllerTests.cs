using System;
using Xunit;
using Moq;
using FissaBissa.Controllers;
using System.Threading.Tasks;
using FissaBissa.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using FissaBissa.Entities;
using FissaBissa.Models;

namespace FissaBissa.Tests
{
    public class AnimalsControllerTests
    {
        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfAnimals()
        {
            // Arrange
            var mockAnimalRepo = new Mock<IAnimalRepository>();
            mockAnimalRepo.Setup(repo => repo.Get())
                .Returns(GetTestAnimals());
            var controller = new AnimalsController(mockAnimalRepo.Object, null);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<AnimalEntity>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Detail_ReturnsAViewResult_WithAnAnimal()
        {
            // Arrange
            var id = Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb");
            var mockAnimalRepo = new Mock<IAnimalRepository>();
            mockAnimalRepo.Setup(repo => repo.Get(id))
                .Returns(GetTestAnimal());
            var controller = new AnimalsController(mockAnimalRepo.Object, null);

            // Act
            var result = await controller.Details(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AnimalEntity>(viewResult.ViewData.Model);
            Assert.Equal(id, model.Id);
        }

        [Fact]
        public async Task Detail_ReturnsNotFound()
        {
            // Arrange
            var controller = new AnimalsController(null, null);

            // Act
            var result = await controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsAViewResult()
        {
            // Arrange
            var mockAnimalRepo = new Mock<IAnimalRepository>();
            mockAnimalRepo.Setup(repo => repo.GetTypes())
                .Returns(GetTestTypes());
            var mockAccessoryRepo = new Mock<IAccessoryRepository>();
            mockAccessoryRepo.Setup(repo => repo.Get())
                .Returns(GetTestAccessories());
            var controller = new AnimalsController(mockAnimalRepo.Object, mockAccessoryRepo.Object);

            // Act
            var result = await controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsAViewResult_WithAnAnimal()
        {
            // Arrange
            var id = Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb");
            var mockAnimalRepo = new Mock<IAnimalRepository>();
            mockAnimalRepo.Setup(repo => repo.Get(id))
                .Returns(GetTestAnimal());
            mockAnimalRepo.Setup(repo => repo.GetTypes())
                .Returns(GetTypes());
            var mockAccessoryRepo = new Mock<IAccessoryRepository>();
            mockAccessoryRepo.Setup(repo => repo.Get())
                .Returns(GetTestAccessories());
            var controller = new AnimalsController(mockAnimalRepo.Object, mockAccessoryRepo.Object);

            // Act
            var result = await controller.Update(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AnimalModel>(viewResult.ViewData.Model);
            Assert.Equal(id, model.Id);
        }

        [Fact]
        public async Task Delete_ReturnsAViewResult_WithAnAnimal()
        {
            // Arrange
            var id = Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb");
            var mockAnimalRepo = new Mock<IAnimalRepository>();
            mockAnimalRepo.Setup(repo => repo.Get(id))
                .Returns(GetTestAnimal());
            var controller = new AnimalsController(mockAnimalRepo.Object, null);

            // Act
            var result = await controller.Delete((Guid?) id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AnimalEntity>(viewResult.ViewData.Model);
            Assert.Equal(id, model.Id);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound()
        {
            // Arrange
            var mockAnimalRepo = new Mock<IAnimalRepository>();
            var mockAccessoryRepo = new Mock<IAccessoryRepository>();
            var controller = new AnimalsController(mockAnimalRepo.Object, mockAccessoryRepo.Object);

            // Act
            var result = await controller.Delete(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsARedirect()
        {
            // Arrange
            var id = Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb");
            var mockAnimalRepo = new Mock<IAnimalRepository>();
            mockAnimalRepo.Setup(repo => repo.Get(id))
                .Returns(GetTestAnimal());
            var mockAccessoryRepo = new Mock<IAccessoryRepository>();
            var controller = new AnimalsController(mockAnimalRepo.Object, mockAccessoryRepo.Object);

            // Act
            var result = await controller.Delete(id);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        private Task<ICollection<AnimalEntity>> GetTestAnimals()
        {
            return Task.FromResult(new List<AnimalEntity>()
            {
                new AnimalEntity()
                {
                    Id = Guid.NewGuid(),
                    Name = "Hond",
                    Price = 500,
                    TypeId = Guid.Parse("bf550047-1eed-479f-a691-bf7d4c22bf17")
                },
                new AnimalEntity()
                {
                    Id = Guid.NewGuid(),
                    Name = "Kat",
                    Price = 500,
                    TypeId = Guid.Parse("bf550047-1eed-479f-a691-bf7d4c22bf17"),
                    Accessories = new List<AnimalAccessoryEntity>()
                    {
                    } as ICollection<AnimalAccessoryEntity>
                }
            } as ICollection<AnimalEntity>);
        }

        private Task<ICollection<AnimalTypeEntity>> GetTestTypes()
        {
            return Task.FromResult(new List<AnimalTypeEntity>()
            {
                new AnimalTypeEntity()
                {
                    Id = Guid.NewGuid(),
                    Name = "Woestijn"
                },
                new AnimalTypeEntity()
                {
                    Id = Guid.NewGuid(),
                    Name = "Strand"
                }
            } as ICollection<AnimalTypeEntity>);
        }

        private Task<ICollection<AccessoryEntity>> GetTestAccessories()
        {
            return Task.FromResult(new List<AccessoryEntity>()
            {
                new AccessoryEntity()
                {
                    Id = Guid.NewGuid(),
                    Name = "band",
                    Price = 500,
                },
                new AccessoryEntity()
                {
                    Id = Guid.NewGuid(),
                    Name = "ketting",
                    Price = 500,
                }
            } as ICollection<AccessoryEntity>);
        }

        private Task<AnimalEntity> GetTestAnimal()
        {
            return Task.FromResult(new AnimalEntity()
            {
                Id = Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb"),
                Name = "Kat",
                Price = 500,
                TypeId = Guid.Parse("bf550047-1eed-479f-a691-bf7d4c22bf17"),
                Accessories = new List<AnimalAccessoryEntity>(),
                Reservations = new List<AnimalReservationEntity>()
            });
        }

        private Task<ICollection<AnimalTypeEntity>> GetTypes()
        {
            return Task.FromResult(new List<AnimalTypeEntity>()
            {
                new AnimalTypeEntity()
                {
                    Id = Guid.NewGuid(),
                    Name = "Woestijn",
                },
                new AnimalTypeEntity()
                {
                    Id = Guid.NewGuid(),
                    Name = "Boerderij",
                }
            } as ICollection<AnimalTypeEntity>);
        }
    }
}
