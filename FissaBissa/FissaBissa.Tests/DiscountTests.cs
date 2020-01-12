using ServiceReference;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FissaBissa.Tests
{
    public class DiscountTests
    {
        [Fact]
        public async Task GetDiscountsAsync_ReturnsTypeDiscount()
        {
            // Arrange
            var service = new ServiceClient();

            // Act
            var result = await service.GetDiscountAsync(new DataModel
            {
                Date = DateTime.Today,
                Animals = new[]
                {
                    new AnimalModel {Name = "test", Type = "test"},
                    new AnimalModel {Name = "test", Type = "test"},
                    new AnimalModel {Name = "test", Type = "test"}
                }
            });

            // Assert
            Assert.True(result.ContainsKey("3x test"));
            Assert.Equal(15, result["3x test"]);
        }

        [Fact]
        public async Task GetDiscountsAsync_ReturnsDuckDiscount()
        {
            // Arrange
            var service = new ServiceClient();
            var total = 0;

            // Act
            for (var i = 0; i < 100; i++)
            {
                var result = await service.GetDiscountAsync(new DataModel
                {
                    Date = DateTime.Today,
                    Animals = new[]
                    {
                        new AnimalModel {Name = "Duck", Type = "test"}
                    }
                });

                if (result.ContainsKey("Duck"))
                {
                    total++;
                }
            }

            // Assert
            Assert.InRange(total, 5, 25);
        }

        [Fact]
        public async Task GetDiscountsAsync_ReturnsDayDiscountMonday()
        {
            // Arrange
            var service = new ServiceClient();

            // Act
            var result = await service.GetDiscountAsync(new DataModel
            {
                Date = GetNextDay(DayOfWeek.Monday),
                Animals = Array.Empty<AnimalModel>()
            });

            // Assert
            Assert.True(result.ContainsKey("Dag"));
            Assert.Equal(15, result["Dag"]);
        }

        [Fact]
        public async Task GetDiscountsAsync_ReturnsDayDiscountTuesday()
        {
            // Arrange
            var service = new ServiceClient();

            // Act
            var result = await service.GetDiscountAsync(new DataModel
            {
                Date = GetNextDay(DayOfWeek.Tuesday),
                Animals = Array.Empty<AnimalModel>()
            });

            // Assert
            Assert.True(result.ContainsKey("Dag"));
            Assert.Equal(15, result["Dag"]);
        }

        [Fact]
        public async Task GetDiscountsAsync_ReturnsAlphabetDiscount()
        {
            // Arrange
            var service = new ServiceClient();

            // Act
            var result = await service.GetDiscountAsync(new DataModel
            {
                Date = DateTime.Today,
                Animals = new[]
                {
                    new AnimalModel {Name = "Abc", Type = "test"},
                    new AnimalModel {Name = "adeF", Type = "test"}
                }
            });

            // Assert
            Assert.True(result.ContainsKey("Letter"));
            Assert.Equal(8, result["Letter"]);
        }

        private DateTime GetNextDay(DayOfWeek day)
        {
            var date = DateTime.Today;

            while (date.DayOfWeek != day)
            {
                date = date.AddDays(1);
            }

            return date;
        }
    }
}
