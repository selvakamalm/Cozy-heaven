using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Controllers;
using ServiceLayer.Services.Interfaces;
using DAL.Models.Main;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CozyHaven.Tests.Controllers
{
    [TestFixture]
    public class BookingControllerTests
    {
        private Mock<IBookingService> _bookingServiceMock = null!;
        private BookingController _controller = null!;

        [SetUp]
        public void SetUp()
        {
            _bookingServiceMock = new Mock<IBookingService>();
            _controller = new BookingController(_bookingServiceMock.Object);
        }

        [Test]
        public async Task GetAllBookings_ShouldReturnOk_WithList()
        {
            // Arrange
            var bookings = new List<Booking>
            {
                new Booking { Id = 1 },
                new Booking { Id = 2 }
            };
            _bookingServiceMock.Setup(s => s.GetAllBookingsAsync())
                               .ReturnsAsync(bookings);

            // Act
            var result = await _controller.GetAllBookings();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var data = okResult?.Value as List<Booking>;
            Assert.That(data?.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetBookingById_ShouldReturnBooking_WhenExists()
        {
            var booking = new Booking { Id = 1 };
            _bookingServiceMock.Setup(s => s.GetBookingByIdAsync(1))
                               .ReturnsAsync(booking);

            var result = await _controller.GetBookingById(1);

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(((Booking)okResult!.Value!).Id, Is.EqualTo(1));
        }

        [Test]
        public async Task GetBookingById_ShouldReturnNotFound_WhenMissing()
        {
            _bookingServiceMock.Setup(s => s.GetBookingByIdAsync(10))
                               .ReturnsAsync((Booking?)null);

            var result = await _controller.GetBookingById(10);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task CreateBooking_ShouldReturnCreatedAtAction()
        {
            var booking = new Booking { Id = 1, RoomId = 2, UserId = 3 };
            _bookingServiceMock.Setup(s => s.AddBookingAsync(It.IsAny<Booking>()))
                               .ReturnsAsync(booking);

            var result = await _controller.CreateBooking(booking);

            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult?.RouteValues?["id"], Is.EqualTo(1));
        }
    }
}
