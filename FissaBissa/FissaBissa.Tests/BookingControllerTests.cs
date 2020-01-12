using System;
using System.Threading.Tasks;
using FissaBissa.Controllers;
using FissaBissa.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FissaBissa.Tests
{
    public class BookingControllerTests
    {
        [Fact]
        public async Task Index_Redirect()
        {
            // Arrange
            var controller = new BookingController(null, null, null, null, null);

            // Act
            var response = await controller.Index(new ReservationModel
            {
                Date = DateTime.Today.AddDays(-1)
            });

            // Assert
            var result = Assert.IsType<ViewResult>(response);
            Assert.Equal("../Home/Index", result.ViewName);
        }
    }
}
