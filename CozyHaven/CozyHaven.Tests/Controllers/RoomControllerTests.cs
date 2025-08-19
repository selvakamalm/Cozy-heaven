using DAL.Models.Main;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ServiceLayer.Controllers;
using ServiceLayer.Services.Interfaces;

namespace CozyHaven.Tests.Controllers
{
    [TestFixture]
    public class RoomControllerTests
    {
        private Mock<IRoomService> _roomServiceMock = null!;
        private RoomController _roomController = null!;

        [SetUp]
        public void SetUp()
        {
            _roomServiceMock = new Mock<IRoomService>();
            _roomController = new RoomController(_roomServiceMock.Object);
        }

        [Test]
        public async Task GetAllRooms_ShouldReturnOk_WithRooms()
        {
            var rooms = new List<Room> { new Room { Id = 1 }, new Room { Id = 2 } };
            _roomServiceMock.Setup(s => s.GetAllRoomsAsync()).ReturnsAsync(rooms);

            var result = await _roomController.GetAllRooms();

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.Value, Is.EqualTo(rooms));
        }

        [Test]
        public async Task GetRoomById_ShouldReturnOk_WhenRoomExists()
        {
            var room = new Room { Id = 1 };
            _roomServiceMock.Setup(s => s.GetRoomByIdAsync(1)).ReturnsAsync(room);

            var result = await _roomController.GetRoomById(1);

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.Value, Is.EqualTo(room));
        }

        [Test]
        public async Task GetRoomById_ShouldReturnNotFound_WhenRoomMissing()
        {
            _roomServiceMock.Setup(s => s.GetRoomByIdAsync(99)).ReturnsAsync((Room?)null);

            var result = await _roomController.GetRoomById(99);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetRoomsByHotelId_ShouldReturnOk()
        {
            var rooms = new List<Room> { new Room { HotelId = 1 } };
            _roomServiceMock.Setup(s => s.GetRoomsByHotelIdAsync(1)).ReturnsAsync(rooms);

            var result = await _roomController.GetRoomsByHotelId(1);

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.Value, Is.EqualTo(rooms));
        }

        [Test]
        public async Task AddRoom_ShouldReturnCreatedAtAction()
        {
            var room = new Room { Id = 1 };
            _roomServiceMock.Setup(s => s.AddRoomAsync(room)).ReturnsAsync(room);

            var result = await _roomController.AddRoom(room);

            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
            Assert.That(createdResult!.ActionName, Is.EqualTo(nameof(_roomController.GetRoomById)));
            Assert.That(createdResult.Value, Is.EqualTo(room));
        }

        [Test]
        public async Task UpdateRoom_ShouldReturnNoContent_WhenSuccess()
        {
            var room = new Room { Id = 1 };
            _roomServiceMock.Setup(s => s.UpdateRoomAsync(1, room)).ReturnsAsync(true);

            var result = await _roomController.UpdateRoom(1, room);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task UpdateRoom_ShouldReturnNotFound_WhenFailed()
        {
            var room = new Room { Id = 1 };
            _roomServiceMock.Setup(s => s.UpdateRoomAsync(1, room)).ReturnsAsync(false);

            var result = await _roomController.UpdateRoom(1, room);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteRoom_ShouldReturnNoContent_WhenSuccess()
        {
            _roomServiceMock.Setup(s => s.DeleteRoomAsync(1)).ReturnsAsync(true);

            var result = await _roomController.DeleteRoom(1);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteRoom_ShouldReturnNotFound_WhenMissing()
        {
            _roomServiceMock.Setup(s => s.DeleteRoomAsync(99)).ReturnsAsync(false);

            var result = await _roomController.DeleteRoom(99);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
