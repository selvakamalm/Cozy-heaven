using NUnit.Framework;
using Moq;
using DAL.DataAccess.Interface;
using DAL.Models.Main;
using ServiceLayer.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CozyHaven.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepoMock = null!;
        private UserService _userService = null!;

        [SetUp]
        public void SetUp()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepoMock.Object);
        }

        [Test]
        public async Task GetAllUsersAsync_ShouldReturnUserList()
        {
            var users = new List<User> {
                new User { Id = 1, Email = "a@example.com" },
                new User { Id = 2, Email = "b@example.com" }
            };

            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            var result = await _userService.GetAllUsersAsync();

            Assert.That(result, Has.Exactly(2).Items);
        }

        [Test]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenExists()
        {
            var user = new User { Id = 1, Email = "test@example.com" };
            _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);

            var result = await _userService.GetUserByIdAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Email, Is.EqualTo("test@example.com"));
        }

        [Test]
        public async Task GetUserByEmailAsync_ShouldReturnUser_WhenExists()
        {
            var user = new User { Email = "user@test.com" };
            _userRepoMock.Setup(r => r.GetByEmailAsync("user@test.com")).ReturnsAsync(user);

            var result = await _userService.GetUserByEmailAsync("user@test.com");

            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Email, Is.EqualTo("user@test.com"));
        }

        [Test]
        public async Task AuthenticateAsync_ShouldReturnUser_WhenCredentialsMatch()
        {
            var user = new User { Email = "admin@test.com", PasswordHash = "pass123" };
            _userRepoMock.Setup(r => r.GetByEmailAndPasswordAsync("admin@test.com", "pass123"))
                         .ReturnsAsync(user);

            var result = await _userService.AuthenticateAsync("admin@test.com", "pass123");

            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Email, Is.EqualTo("admin@test.com"));
        }

        [Test]
        public async Task AddUserAsync_ShouldCallRepository()
        {
            var user = new User { Id = 1, Email = "new@test.com" };

            _userRepoMock.Setup(r => r.AddAsync(user)).Returns(Task.CompletedTask);
            _userRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            await _userService.AddUserAsync(user);

            _userRepoMock.Verify(r => r.AddAsync(user), Times.Once);
            _userRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateUserAsync_ShouldCallUpdateAndSave()
        {
            var user = new User { Id = 2, Email = "update@test.com" };

            _userRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            await _userService.UpdateUserAsync(user);

            _userRepoMock.Verify(r => r.Update(user), Times.Once);
            _userRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteUserAsync_ShouldCallDelete_WhenUserExists()
        {
            var user = new User { Id = 3 };

            _userRepoMock.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(user);
            _userRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            await _userService.DeleteUserAsync(3);

            _userRepoMock.Verify(r => r.Delete(user), Times.Once);
            _userRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteUserAsync_ShouldNotCallDelete_WhenUserNotFound()
        {
            _userRepoMock.Setup(r => r.GetByIdAsync(10)).ReturnsAsync((User?)null);

            await _userService.DeleteUserAsync(10);

            _userRepoMock.Verify(r => r.Delete(It.IsAny<User>()), Times.Never);
            _userRepoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }
}
