using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Controllers;
using ServiceLayer.Services.Interfaces;
using DAL.Models.Main;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CozyHaven.Tests.Controllers
{
    [TestFixture]
    public class HotelControllerTests
    {
        private Mock<IHotelService> _hotelServiceMock = null!;
        private HotelController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _hotelServiceMock = new Mock<IHotelService>();
            _controller = new HotelController(_hotelServiceMock.Object);
        }

        [Test]
        public async Task GetAllHotels_ShouldReturnOk()
        {
            var hotels = new List<Hotel> {
                new Hotel { Id = 1, Name = "A" },
                new Hotel { Id = 2, Name = "B" }
            };

            _hotelServiceMock.Setup(s => s.GetAllHotelsAsync()).ReturnsAsync(hotels);

            var result = await _controller.GetAllHotels();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetHotelById_ShouldReturnOk_WhenFound()
        {
            _hotelServiceMock.Setup(s => s.GetHotelByIdAsync(1))
                .ReturnsAsync(new Hotel { Id = 1 });

            var result = await _controller.GetHotelById(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetHotelById_ShouldReturnNotFound_WhenMissing()
        {
            _hotelServiceMock.Setup(s => s.GetHotelByIdAsync(99)).ReturnsAsync((Hotel?)null);

            var result = await _controller.GetHotelById(99);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetHotelsByLocation_ShouldReturnOk()
        {
            var location = "Delhi";
            _hotelServiceMock.Setup(s => s.GetHotelsByLocationAsync(location))
                .ReturnsAsync(new List<Hotel> { new Hotel { Id = 1, Location = location } });

            var result = await _controller.GetHotelsByLocation(location);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetHotelsByOwnerId_ShouldReturnOk_WhenFound()
        {
            _hotelServiceMock.Setup(s => s.GetHotelsByOwnerAsync(4))
                .ReturnsAsync(new List<Hotel> { new Hotel { Id = 1, OwnerId = 4 } });

            var result = await _controller.GetHotelsByOwnerId(4);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetHotelsByOwnerId_ShouldReturnNotFound_WhenEmpty()
        {
            _hotelServiceMock.Setup(s => s.GetHotelsByOwnerAsync(4)).ReturnsAsync(new List<Hotel>());

            var result = await _controller.GetHotelsByOwnerId(4);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task CreateHotel_ShouldReturnCreatedResult()
        {
            var hotel = new Hotel { Id = 10, Name = "Test" };

            _hotelServiceMock.Setup(s => s.AddHotelAsync(hotel)).ReturnsAsync(hotel);

            var result = await _controller.CreateHotel(hotel);

            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
        }

        [Test]
        public async Task UpdateHotel_ShouldReturnNoContent_WhenUpdated()
        {
            var hotel = new Hotel { Id = 5, Name = "Update" };

            _hotelServiceMock.Setup(s => s.UpdateHotelAsync(5, hotel)).ReturnsAsync(true);

            var result = await _controller.UpdateHotel(5, hotel);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task UpdateHotel_ShouldReturnBadRequest_WhenIdMismatch()
        {
            var hotel = new Hotel { Id = 7 };

            var result = await _controller.UpdateHotel(6, hotel);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task UpdateHotel_ShouldReturnNotFound_WhenNotUpdated()
        {
            var hotel = new Hotel { Id = 99 };

            _hotelServiceMock.Setup(s => s.UpdateHotelAsync(99, hotel)).ReturnsAsync(false);

            var result = await _controller.UpdateHotel(99, hotel);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteHotel_ShouldReturnNoContent_WhenDeleted()
        {
            _hotelServiceMock.Setup(s => s.DeleteHotelAsync(8)).ReturnsAsync(true);

            var result = await _controller.DeleteHotel(8);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteHotel_ShouldReturnNotFound_WhenNotFound()
        {
            _hotelServiceMock.Setup(s => s.DeleteHotelAsync(100)).ReturnsAsync(false);

            var result = await _controller.DeleteHotel(100);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
