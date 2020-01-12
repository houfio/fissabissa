using ServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
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
            // var service = new ServiceClient();

            // Act
            var result = GetDiscount(new DataModel
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
            Assert.Equal(10, result["3x test"]);
        }

        [Fact]
        public async Task GetDiscountsAsync_ReturnsDuckDiscount()
        {
            // Arrange
            // var service = new ServiceClient();
            var total = 0;

            // Act
            for (var i = 0; i < 100; i++)
            {
                var result = GetDiscount(new DataModel
                {
                    Date = DateTime.Today,
                    Animals = new[]
                    {
                        new AnimalModel {Name = "Eend", Type = "test"}
                    }
                });

                if (result.ContainsKey("Eend"))
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
            var result = GetDiscount(new DataModel
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
            // var service = new ServiceClient();

            // Act
            var result = GetDiscount(new DataModel
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
            // var service = new ServiceClient();

            // Act
            var result = GetDiscount(new DataModel
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

        // :(

        private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";

        public Dictionary<string, int> GetDiscount(DataModel model)
        {
            var discounts = model.Animals
                .Aggregate(new Dictionary<string, int>(), (result, current) =>
                {
                    result[current.Type] = result.TryGetValue(current.Type, out var amount) ? amount + 1 : 1;

                    return result;
                })
                .Where(p => p.Value >= 3)
                .ToDictionary(p => $"{p.Value}x {p.Key}", p => 10);

            if (model.Animals.Any(a => a.Name == "Eend") && new Random().Next(0, 5) == 0)
            {
                discounts["Eend"] = 50;
            }

            if (model.Date.DayOfWeek == DayOfWeek.Monday || model.Date.DayOfWeek == DayOfWeek.Tuesday)
            {
                discounts["Dag"] = 15;
            }

            var discount = model.Animals.Select(a => Enumerable.Range(0, Alphabet.Length).Aggregate(0, (result, current) => result == current && a.Name.ToLowerInvariant().Contains(Alphabet[current]) ? result + 1 : result) * 2).Sum();

            if (discount > 0)
            {
                discounts["Letter"] = discount;
            }

            return discounts;
        }
    }
}
