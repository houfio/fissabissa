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
            var mockAccessoryRepo = new Mock<IAccessoryRepository>();
            mockAccessoryRepo.Setup(repo => repo.Get())
                .Returns(GetTestAccessories());
            var controller = new AccessoriesController(mockAccessoryRepo.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ICollection<AccessoryEntity>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Detail_ReturnsAViewResult_WithAccessory()
        {
            // Arrange
            var mockAccessoryRepo = new Mock<IAccessoryRepository>();
            mockAccessoryRepo.Setup(repo => repo.Get(Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb")))
                .Returns(GetTestAccessory());
            var controller = new AccessoriesController(mockAccessoryRepo.Object);

            // Act
            var result = await controller.Details(Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb"));

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AccessoryEntity>(viewResult.ViewData.Model);
            Assert.Equal(Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb"), model.Id);
        }

        [Fact]
        public async Task Detail_ReturnsNotFound()
        {
            // Arrange
            var controller = new AccessoriesController(null);

            // Act
            var result = await controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsAViewResult()
        {
            var mockAccessoryRepo = new Mock<IAccessoryRepository>();
            var controller = new AccessoriesController(mockAccessoryRepo.Object);

            // Act
            var result = controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsAViewResult_WithAccessory()
        {
            // Arrange
            var mockAccessoryRepo = new Mock<IAccessoryRepository>();
            mockAccessoryRepo.Setup(repo => repo.Get(Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb")))
                .Returns(GetTestAccessory());
            var controller = new AccessoriesController(mockAccessoryRepo.Object);

            // Act
            var result = await controller.Update((Guid?)Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb"));

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AccessoryModel>(viewResult.ViewData.Model);
            Assert.Equal(Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb"), model.Id);
        }

        [Fact]
        public async Task Delete_ReturnsAViewResult_WithAccessory()
        {
            // Arrange
            var mockAccessoryRepo = new Mock<IAccessoryRepository>();
            mockAccessoryRepo.Setup(repo => repo.Get(Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb")))
                .Returns(GetTestAccessory());
            var controller = new AccessoriesController(mockAccessoryRepo.Object);

            // Act
            var result = await controller.Delete((Guid?)Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb"));

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AccessoryEntity>(viewResult.ViewData.Model);
            Assert.Equal(Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb"), model.Id);
        }

        [Fact]
        public async Task Delete_ReturnsARedirect()
        {
            // Arrange
            var mockAccessoryRepo = new Mock<IAccessoryRepository>();
            mockAccessoryRepo.Setup(repo => repo.Get(Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb")))
                .Returns(GetTestAccessory());
            var controller = new AccessoriesController(mockAccessoryRepo.Object);

            // Act
            var result = await controller.DeleteConfirmed(Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb"));

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        private Task<ICollection<AccessoryEntity>> GetTestAccessories()
        {
            return Task.FromResult(new List<AccessoryEntity>()
            {
                new AccessoryEntity()
                {
                    Id = Guid.NewGuid(),
                    Name = "Halsband",
                    Price = 500,
                },
                new AccessoryEntity()
                {
                    Id = Guid.NewGuid(),
                    Name = "Ketting",
                    Price = 500,
                },
            } as ICollection<AccessoryEntity>);
        }

        private Task<AccessoryEntity> GetTestAccessory()
        {
            return Task.FromResult(new AccessoryEntity()
            {
                Id = Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb"),
                Name = "Ketting",
                Price = 500,
            });
        }
    }
}
