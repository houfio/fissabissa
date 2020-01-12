using FissaBissa.Entities;
using FissaBissa.Utilities;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace FissaBissa.Tests
{
    public class IdentitySeederTests
    {
        [Fact]
        public void Seed_AddRoles()
        {
            // Arrange
            var roleStore = new Mock<IRoleStore<RoleEntity>>();
            var mock = new Mock<RoleManager<RoleEntity>>(roleStore.Object, null, null, null, null);
            mock.Setup(manager => manager.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(false);
            
            // Act
            IdentitySeeder.Seed(mock.Object);

            // Assert
            mock.Verify(manager => manager.CreateAsync(It.IsAny<RoleEntity>()), Times.Once);
        }

        [Fact]
        public void Seed_DontAddRoles()
        {
            // Arrange
            var roleStore = new Mock<IRoleStore<RoleEntity>>();
            var mock = new Mock<RoleManager<RoleEntity>>(roleStore.Object, null, null, null, null);
            mock.Setup(manager => manager.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            IdentitySeeder.Seed(mock.Object);

            // Assert
            mock.Verify(manager => manager.CreateAsync(It.IsAny<RoleEntity>()), Times.Never);
        }
    }
}
