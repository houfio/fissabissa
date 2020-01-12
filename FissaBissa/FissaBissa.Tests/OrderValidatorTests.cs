using System;
using System.Collections.Generic;
using FissaBissa.Entities;
using FissaBissa.Utilities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Xunit;

namespace FissaBissa.Tests
{
    public class OrderValidatorTests
    {
        [Fact]
        public void ValidateOrder_Valid()
        {
            // Arrange
            var key = "test";
            var state = new ModelStateDictionary();

            // Act
            OrderValidator.ValidateOrder(key, new List<AnimalEntity>
            {
                CreateAnimal("Aap", "Jungle")
            }, DateTime.Today, state);

            // Assert
            Assert.True(state.IsValid);
        }

        [Fact]
        public void ValidateOrder_NoAnimals()
        {
            // Arrange
            var key = "test";
            var state = new ModelStateDictionary();

            // Act
            OrderValidator.ValidateOrder(key, new List<AnimalEntity>(), DateTime.Today, state);

            // Assert
            Assert.False(state.IsValid);
            Assert.Equal(1, state.ErrorCount);
        }

        [Fact]
        public void ValidateOrder_InvalidCombination()
        {
            // Arrange
            var key = "test";
            var state = new ModelStateDictionary();

            // Act
            OrderValidator.ValidateOrder(key, new List<AnimalEntity>
            {
                CreateAnimal("test", "Boerderij"),
                CreateAnimal("Leeuw", "test"),
                CreateAnimal("Ijsbeer", "test")
            }, DateTime.Today, state);

            // Assert
            Assert.False(state.IsValid);
            Assert.Equal(1, state.ErrorCount);
        }

        [Fact]
        public void ValidateOrder_InvalidDayMonday()
        {
            // Arrange
            var key = "test";
            var state = new ModelStateDictionary();

            // Act
            OrderValidator.ValidateOrder(key, new List<AnimalEntity>
            {
                CreateAnimal("Pinguïn", "test")
            }, GetNextDay(DayOfWeek.Monday), state);

            // Assert
            Assert.True(state.IsValid);
        }

        [Fact]
        public void ValidateOrder_InvalidDaySaturday()
        {
            // Arrange
            var key = "test";
            var state = new ModelStateDictionary();

            // Act
            OrderValidator.ValidateOrder(key, new List<AnimalEntity>
            {
                CreateAnimal("Pinguïn", "test")
            }, GetNextDay(DayOfWeek.Saturday), state);

            // Assert
            Assert.False(state.IsValid);
            Assert.Equal(1, state.ErrorCount);
        }

        [Fact]
        public void ValidateOrder_InvalidDaySunday()
        {
            // Arrange
            var key = "test";
            var state = new ModelStateDictionary();

            // Act
            OrderValidator.ValidateOrder(key, new List<AnimalEntity>
            {
                CreateAnimal("Pinguïn", "test")
            }, GetNextDay(DayOfWeek.Sunday), state);

            // Assert
            Assert.False(state.IsValid);
            Assert.Equal(1, state.ErrorCount);
        }

        [Fact]
        public void ValidateOrder_InvalidMonthDesert()
        {
            // Arrange
            var key = "test";
            var state = new ModelStateDictionary();

            // Act
            OrderValidator.ValidateOrder(key, new List<AnimalEntity>
            {
                CreateAnimal("test", "Woestijn")
            }, GetNextMonth(1), state);

            // Assert
            Assert.False(state.IsValid);
            Assert.Equal(1, state.ErrorCount);
        }

        [Fact]
        public void ValidateOrder_InvalidMonthSnow()
        {
            // Arrange
            var key = "test";
            var state = new ModelStateDictionary();

            // Act
            OrderValidator.ValidateOrder(key, new List<AnimalEntity>
            {
                CreateAnimal("test", "Sneeuw")
            }, GetNextMonth(6), state);

            // Assert
            Assert.False(state.IsValid);
            Assert.Equal(1, state.ErrorCount);
        }

        private AnimalEntity CreateAnimal(string name, string type)
        {
            return new AnimalEntity
            {
                Name = name,
                Type = new AnimalTypeEntity
                {
                    Name = type
                }
            };
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

        private DateTime GetNextMonth(int month)
        {
            var date = DateTime.Today;

            while (date.Month != month)
            {
                date = date.AddMonths(1);
            }

            return date;
        }
    }
}
