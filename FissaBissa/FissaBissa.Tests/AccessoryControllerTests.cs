using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FissaBissa.Controllers;
using FissaBissa.Entities;
using FissaBissa.Models;
using FissaBissa.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

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
            var id = Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb");
            var mockAccessoryRepo = new Mock<IAccessoryRepository>();
            mockAccessoryRepo.Setup(repo => repo.Get(id))
                .Returns(GetTestAccessory());
            var controller = new AccessoriesController(mockAccessoryRepo.Object);

            // Act
            var result = await controller.Details(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AccessoryEntity>(viewResult.ViewData.Model);
            Assert.Equal(id, model.Id);
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
            var controller = new AccessoriesController(null);

            // Act
            var result = controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsAViewResult_WithAccessory()
        {
            // Arrange
            var id = Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb");
            var mockAccessoryRepo = new Mock<IAccessoryRepository>();
            mockAccessoryRepo.Setup(repo => repo.Get(id))
                .Returns(GetTestAccessory());
            var controller = new AccessoriesController(mockAccessoryRepo.Object);

            // Act
            var result = await controller.Update(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AccessoryModel>(viewResult.ViewData.Model);
            Assert.Equal(id, model.Id);
        }

        [Fact]
        public async Task Delete_ReturnsAViewResult_WithAccessory()
        {
            // Arrange
            var id = Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb");
            var mockAccessoryRepo = new Mock<IAccessoryRepository>();
            mockAccessoryRepo.Setup(repo => repo.Get(id))
                .Returns(GetTestAccessory());
            var controller = new AccessoriesController(mockAccessoryRepo.Object);

            // Act
            var result = await controller.Delete(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AccessoryEntity>(viewResult.ViewData.Model);
            Assert.Equal(id, model.Id);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_IdNull()
        {
            // Arrange
            var mockAccessoryRepo = new Mock<IAccessoryRepository>();
            mockAccessoryRepo.Setup(repo => repo.Get(Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb")))
                .Returns(GetTestAccessory());
            var controller = new AccessoriesController(mockAccessoryRepo.Object);

            // Act
            var result = await controller.Delete(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_EntityNull()
        {
            // Arrange
            var id = Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb");
            var mockAccessoryRepo = new Mock<IAccessoryRepository>();
            mockAccessoryRepo.Setup(repo => repo.Get(id))
                .Returns(GetTestAccessory());
            var controller = new AccessoriesController(mockAccessoryRepo.Object);

            // Act
            var result = await controller.Delete(id);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_ReturnsARedirect()
        {
            // Arrange
            var id = Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb");
            var mockAccessoryRepo = new Mock<IAccessoryRepository>();
            mockAccessoryRepo.Setup(repo => repo.Get(id))
                .Returns(GetTestAccessory());
            var controller = new AccessoriesController(mockAccessoryRepo.Object);

            // Act
            var result = await controller.DeleteConfirmed(id);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        private Task<ICollection<AccessoryEntity>> GetTestAccessories()
        {
            return Task.FromResult(new List<AccessoryEntity>
            {
                new AccessoryEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Halsband",
                    Price = 500
                },
                new AccessoryEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Ketting",
                    Price = 500
                }
            } as ICollection<AccessoryEntity>);
        }

        private Task<AccessoryEntity> GetTestAccessory()
        {
            return Task.FromResult(new AccessoryEntity
            {
                Id = Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb"),
                Name = "Ketting",
                Price = 500
            });
        }

        private AccessoryModel GetTestAccessoryModel()
        {
            return new AccessoryModel
            {
                Id = Guid.Parse("f0652b93-1728-43f2-8bf7-81d4dadfedfb"),
                Name = "",
                Price = 500,
                Image = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("dummy")), 0, 0, "data", "dummy.txt")
            };
        }
    }
}
