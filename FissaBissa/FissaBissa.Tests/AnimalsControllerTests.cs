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
            var mockAccessoryRepo = new Mock<IAccessoryRepository>();
            var controller = new AnimalsController(mockAnimalRepo.Object, mockAccessoryRepo.Object);

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
            var mockAnimalRepo = new Mock<IAnimalRepository>();
            mockAnimalRepo.Setup(repo => repo.Get(Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb")))
                .Returns(GetTestAnimal());
            var mockAccessoryRepo = new Mock<IAccessoryRepository>();
            var controller = new AnimalsController(mockAnimalRepo.Object, mockAccessoryRepo.Object);

            // Act
            var result = await controller.Details(Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb"));

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AnimalEntity>(viewResult.ViewData.Model);
            Assert.Equal(Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb"), model.Id);
        }

       

        [Fact]
        public async Task Create_ReturnsAViewResult()
        {
            // Arrange
            var mockAnimalRepo = new Mock<IAnimalRepository>();
            mockAnimalRepo.Setup(repo => repo.Get(Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb")))
                .Returns(GetTestAnimal());
            var mockAccessoryRepo = new Mock<IAccessoryRepository>();
            mockAccessoryRepo.Setup(repo => repo.Get())
                .Returns(GetTestAccessories());
            var controller = new AnimalsController(mockAnimalRepo.Object, mockAccessoryRepo.Object);

            // Act
            var result = await controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<AnimalModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Update_ReturnsAViewResult_WithAnAnimal()
        {
            // Arrange
            var mockAnimalRepo = new Mock<IAnimalRepository>();
            mockAnimalRepo.Setup(repo => repo.Get(Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb")))
                .Returns(GetTestAnimal());
            var mockAccessoryRepo = new Mock<IAccessoryRepository>();
            mockAccessoryRepo.Setup(repo => repo.Get())
                .Returns(GetTestAccessories());
            var controller = new AnimalsController(mockAnimalRepo.Object, mockAccessoryRepo.Object);

            // Act
            var result = await controller.Update(Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb"));

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AnimalModel>(viewResult.ViewData.Model);
            Assert.Equal(Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb"), model.Id);
        }

        [Fact]
        public async Task Delete_ReturnsAViewResult_WithAnAnimal()
        {
            // Arrange
            var mockAnimalRepo = new Mock<IAnimalRepository>();
            mockAnimalRepo.Setup(repo => repo.Get(Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb")))
                .Returns(GetTestAnimal());
            var mockAccessoryRepo = new Mock<IAccessoryRepository>();
            var controller = new AnimalsController(mockAnimalRepo.Object, mockAccessoryRepo.Object);

            // Act
            var result = await controller.Delete((Guid?)Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb"));

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AnimalEntity>(viewResult.ViewData.Model);
            Assert.Equal(Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb"), model.Id);
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
            var mockAnimalRepo = new Mock<IAnimalRepository>();
            mockAnimalRepo.Setup(repo => repo.Get(Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb")))
                .Returns(GetTestAnimal());
            var mockAccessoryRepo = new Mock<IAccessoryRepository>();
            var controller = new AnimalsController(mockAnimalRepo.Object, mockAccessoryRepo.Object);

            // Act
            var result = await controller.Delete(Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb"));

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
                    TypeId = Guid.Parse("bf550047-1eed-479f-a691-bf7d4c22bf17")
                }
            } as ICollection<AnimalEntity>);
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
                TypeId = Guid.Parse("bf550047-1eed-479f-a691-bf7d4c22bf17")
            });
        }
    }
}
