using NUnit.Framework;
using NUnit.Framework.Legacy;
using Moq;
using DAL.Models.Main;
using DAL.DataAccess.Interface;
using ServiceLayer.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CozyHaven.Tests.Services
{
    [TestFixture]
    public class BookingServiceTests
    {
        private Mock<IBookingRepository> _bookingRepoMock = null!;
        private BookingService _bookingService = null!;

        [SetUp]
        public void SetUp()
        {
            _bookingRepoMock = new Mock<IBookingRepository>();
            _bookingService = new BookingService(_bookingRepoMock.Object);
        }

        [Test]
        public async Task GetAllBookingsAsync_ShouldReturnBookings()
        {
            // Arrange
            var fakeBookings = new List<Booking>
    {
        new Booking { Id = 1, UserId = 101 },
        new Booking { Id = 2, UserId = 102 }
    };

            _bookingRepoMock.Setup(repo => repo.GetAllAsync())
                            .ReturnsAsync(fakeBookings);

            // Act
            var result = await _bookingService.GetAllBookingsAsync();

            // Assert
            ClassicAssert.IsNotNull(result);

            var resultList = result.ToList(); 
            Assert.That(resultList, Has.Count.EqualTo(2));
        }


        [Test]
        public async Task GetBookingByIdAsync_ShouldReturnBooking_WhenExists()
        {
            var booking = new Booking { Id = 1, RoomId = 5 };
            _bookingRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(booking);

            var result = await _bookingService.GetBookingByIdAsync(1);

            ClassicAssert.IsNotNull(result);
            Assert.That(result?.RoomId, Is.EqualTo(5));
        }

        [Test]
        public async Task AddBookingAsync_ShouldReturnCreatedBooking()
        {
            var booking = new Booking { Id = 1, RoomId = 5 };

            _bookingRepoMock.Setup(r => r.AddAsync(It.IsAny<Booking>())).Returns(Task.CompletedTask);
            _bookingRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _bookingService.AddBookingAsync(booking);

            Assert.That(result, Is.EqualTo(booking));
            _bookingRepoMock.Verify(r => r.AddAsync(booking), Times.Once);
            _bookingRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteBookingAsync_ShouldReturnFalse_WhenNotFound()
        {
            _bookingRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Booking?)null);

            var result = await _bookingService.DeleteBookingAsync(99);

            ClassicAssert.IsFalse(result);
        }

        [Test]
        public async Task DeleteBookingAsync_ShouldReturnTrue_WhenDeleted()
        {
            var booking = new Booking { Id = 1 };

            _bookingRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(booking);
            _bookingRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _bookingService.DeleteBookingAsync(1);

            ClassicAssert.IsTrue(result);
            _bookingRepoMock.Verify(r => r.Delete(booking), Times.Once);
        }
    }
}
