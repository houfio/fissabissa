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
    public class AccessoryControllerTests
    {
        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfAccessories()
        {
            // Arrange
            var mockAnimalRepo = new Mock<IAccessoryRepository>();
            mockAnimalRepo.Setup(repo => repo.Get())
                .Returns(GetTestAccessories());
            var mockAccessoryRepo = new Mock<IAccessoryRepository>();
            var controller = new AccessoriesController(mockAccessoryRepo.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ICollection<AccessoryEntity>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        private Task<ICollection<AccessoryEntity>> GetTestAccessories()
        {
            return Task.FromResult(new List<AccessoryEntity>()
            {
                new AccessoryEntity()
                {
                    Id = Guid.NewGuid(),
                    Name = "halsband",
                    Price = 500,
                },
                new AccessoryEntity()
                {
                    Id = Guid.NewGuid(),
                    Name = "ketting",
                    Price = 500,
                },
            } as ICollection<AccessoryEntity>); ;
        }
    }
}
