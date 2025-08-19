using DAL.DataAccess.Interface;
using DAL.Models.Main;
using Moq;
using NUnit.Framework.Legacy;
using ServiceLayer.Services.Implementation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CozyHaven.Tests.Services
{
    [TestFixture]
    public class HotelServiceTests
    {
        private Mock<IHotelRepository> _hotelRepoMock = null!;
        private HotelService _hotelService = null!;

        [SetUp]
        public void SetUp()
        {
            _hotelRepoMock = new Mock<IHotelRepository>();
            _hotelService = new HotelService(_hotelRepoMock.Object);
        }

        [Test]
        public async Task GetAllHotelsAsync_ShouldReturnHotels()
        {
            var hotels = new List<Hotel> { new Hotel { Id = 1 }, new Hotel { Id = 2 } };
            _hotelRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(hotels);

            var result = await _hotelService.GetAllHotelsAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetHotelByIdAsync_ShouldReturnHotel_WhenFound()
        {
            var hotel = new Hotel { Id = 1 };
            _hotelRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(hotel);

            var result = await _hotelService.GetHotelByIdAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task GetHotelByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            _hotelRepoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync((Hotel?)null);

            var result = await _hotelService.GetHotelByIdAsync(5);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task AddHotelAsync_ShouldReturnHotel()
        {
            var hotel = new Hotel { Id = 1, Name = "Test" };

            _hotelRepoMock.Setup(r => r.AddAsync(hotel)).Returns(Task.CompletedTask);
            _hotelRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _hotelService.AddHotelAsync(hotel);

            Assert.That(result, Is.EqualTo(hotel));
        }

        [Test]
        public async Task UpdateHotelAsync_ShouldReturnTrue_WhenUpdated()
        {
            var hotel = new Hotel { Id = 1, Name = "Updated" };

            _hotelRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Hotel { Id = 1 });
            _hotelRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _hotelService.UpdateHotelAsync(1, hotel);

            ClassicAssert.IsTrue(result);
        }

        [Test]
        public async Task UpdateHotelAsync_ShouldReturnFalse_WhenNotFound()
        {
            _hotelRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync((Hotel?)null);

            var result = await _hotelService.UpdateHotelAsync(2, new Hotel { Id = 2 });

            ClassicAssert.IsFalse(result);
        }

        [Test]
        public async Task DeleteHotelAsync_ShouldReturnTrue_WhenDeleted()
        {
            var hotel = new Hotel { Id = 1 };

            _hotelRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(hotel);
            _hotelRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _hotelService.DeleteHotelAsync(1);

            ClassicAssert.IsTrue(result);
            _hotelRepoMock.Verify(r => r.Delete(hotel), Times.Once);
        }

        [Test]
        public async Task DeleteHotelAsync_ShouldReturnFalse_WhenNotFound()
        {
            _hotelRepoMock.Setup(r => r.GetByIdAsync(10)).ReturnsAsync((Hotel?)null);

            var result = await _hotelService.DeleteHotelAsync(10);

            ClassicAssert.IsFalse(result);
        }

        [Test]
        public async Task GetHotelsByLocationAsync_ShouldReturnList()
        {
            var location = "Chennai";
            var hotels = new List<Hotel> { new Hotel { Location = location } };

            _hotelRepoMock.Setup(r => r.GetHotelsByLocationAsync(location)).ReturnsAsync(hotels);

            var result = await _hotelService.GetHotelsByLocationAsync(location);

            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetHotelsByOwnerAsync_ShouldReturnList()
        {
            var hotels = new List<Hotel> { new Hotel { OwnerId = 5 } };

            _hotelRepoMock.Setup(r => r.GetHotelsByOwnerAsync(5)).ReturnsAsync(hotels);

            var result = await _hotelService.GetHotelsByOwnerAsync(5);

            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetHotelByNameAsync_ShouldReturnHotel()
        {
            var name = "Cozy Stay";
            var hotel = new Hotel { Id = 1, Name = name };

            _hotelRepoMock.Setup(r => r.GetHotelByNameAsync(name)).ReturnsAsync(hotel);

            var result = await _hotelService.GetHotelByNameAsync(name);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo(name));
        }
    }
}
