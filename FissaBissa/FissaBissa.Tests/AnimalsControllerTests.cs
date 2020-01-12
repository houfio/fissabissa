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
    }
}
