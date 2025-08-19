using DAL.DataAccess.Interface;
using DAL.Models.Main;
using Moq;
using NUnit.Framework.Legacy;
using ServiceLayer.Services.Implementation;

namespace CozyHaven.Tests.Services
{
    [TestFixture]
    public class RoomServiceTests
    {
        private Mock<IRoomRepository> _roomRepoMock = null!;
        private RoomService _roomService = null!;

        [SetUp]
        public void SetUp()
        {
            _roomRepoMock = new Mock<IRoomRepository>();
            _roomService = new RoomService(_roomRepoMock.Object);
        }

        [Test]
        public async Task GetAllRoomsAsync_ShouldReturnRooms()
        {
            var fakeRooms = new List<Room> { new Room { Id = 1 }, new Room { Id = 2 } };
            _roomRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(fakeRooms);

            var result = await _roomService.GetAllRoomsAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetRoomByIdAsync_ShouldReturnRoom_WhenExists()
        {
            var room = new Room { Id = 1 };
            _roomRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(room);

            var result = await _roomService.GetRoomByIdAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task GetRoomByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            _roomRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Room?)null);

            var result = await _roomService.GetRoomByIdAsync(99);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetRoomsByHotelIdAsync_ShouldReturnRooms()
        {
            var rooms = new List<Room> { new Room { HotelId = 5 } };
            _roomRepoMock.Setup(r => r.GetRoomsByHotelIdAsync(5)).ReturnsAsync(rooms);

            var result = await _roomService.GetRoomsByHotelIdAsync(5);

            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task AddRoomAsync_ShouldReturnRoom()
        {
            var room = new Room { Id = 1, RoomSize = "Deluxe" };

            _roomRepoMock.Setup(r => r.AddAsync(room)).Returns(Task.CompletedTask);
            _roomRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _roomService.AddRoomAsync(room);

            Assert.That(result, Is.EqualTo(room));
        }

        [Test]
        public async Task UpdateRoomAsync_ShouldReturnTrue_WhenRoomExists()
        {
            var updatedRoom = new Room
            {
                Id = 1,
                HotelId = 2,
                RoomSize = "Luxury",
                BedType = "King",
                MaxOccupancy = 3,
                BaseFare = 5000,
                IsAC = true
            };

            _roomRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Room { Id = 1 });
            _roomRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _roomService.UpdateRoomAsync(1, updatedRoom);

            ClassicAssert.IsTrue(result);
        }

        [Test]
        public async Task UpdateRoomAsync_ShouldReturnFalse_WhenRoomNotFound()
        {
            _roomRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Room?)null);

            var result = await _roomService.UpdateRoomAsync(99, new Room());

            ClassicAssert.IsFalse(result);
        }

        [Test]
        public async Task DeleteRoomAsync_ShouldReturnTrue_WhenRoomExists()
        {
            var room = new Room { Id = 1 };
            _roomRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(room);
            _roomRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _roomService.DeleteRoomAsync(1);

            ClassicAssert.IsTrue(result);
            _roomRepoMock.Verify(r => r.Delete(room), Times.Once);
        }

        [Test]
        public async Task DeleteRoomAsync_ShouldReturnFalse_WhenRoomNotFound()
        {
            _roomRepoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Room?)null);

            var result = await _roomService.DeleteRoomAsync(999);

            ClassicAssert.IsFalse(result);
        }
    }
}
