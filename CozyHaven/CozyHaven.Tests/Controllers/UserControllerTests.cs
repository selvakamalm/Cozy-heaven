using NUnit.Framework;
using Moq;
using ServiceLayer.Controllers;
using ServiceLayer.Services.Interfaces;
using DAL.Models.Main;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CozyHaven.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IUserService> _userServiceMock = null!;
        private UserController _controller = null!;

        [SetUp]
        public void SetUp()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UserController(_userServiceMock.Object);
        }

        [Test]
        public async Task GetAllUsers_ShouldReturnOk_WithUserList()
        {
            var users = new List<User> {
                new User { Id = 1, Email = "a@example.com" },
                new User { Id = 2, Email = "b@example.com" }
            };

            _userServiceMock.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(users);

            var result = await _controller.GetAllUsers();

            var ok = result as OkObjectResult;
            Assert.That(ok, Is.Not.Null);
            var data = ok?.Value as List<User>;
            Assert.That(data?.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetUserById_ShouldReturnOk_WhenFound()
        {
            var user = new User { Id = 1 };
            _userServiceMock.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync(user);

            var result = await _controller.GetUserById(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetUserById_ShouldReturnNotFound_WhenMissing()
        {
            _userServiceMock.Setup(s => s.GetUserByIdAsync(10)).ReturnsAsync((User?)null);

            var result = await _controller.GetUserById(10);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetUserByEmail_ShouldReturnOk_WhenFound()
        {
            var user = new User { Email = "test@example.com" };
            _userServiceMock.Setup(s => s.GetUserByEmailAsync("test@example.com")).ReturnsAsync(user);

            var result = await _controller.GetUserByEmail("test@example.com");

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetUserByEmail_ShouldReturnNotFound_WhenMissing()
        {
            _userServiceMock.Setup(s => s.GetUserByEmailAsync("notfound@example.com")).ReturnsAsync((User?)null);

            var result = await _controller.GetUserByEmail("notfound@example.com");

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task CreateUser_ShouldReturnCreatedAtAction()
        {
            var user = new User { Id = 3, Email = "new@user.com" };
            _userServiceMock.Setup(s => s.AddUserAsync(user)).Returns(Task.CompletedTask);

            var result = await _controller.CreateUser(user);

            var created = result as CreatedAtActionResult;
            Assert.That(created, Is.Not.Null);
            Assert.That(created?.RouteValues?["id"], Is.EqualTo(3));
        }

        [Test]
        public async Task UpdateUser_ShouldReturnNoContent()
        {
            var user = new User { Id = 4, Email = "update@test.com" };
            _userServiceMock.Setup(s => s.UpdateUserAsync(user)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateUser(4, user);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task UpdateUser_ShouldReturnBadRequest_WhenIdMismatch()
        {
            var user = new User { Id = 4 };
            var result = await _controller.UpdateUser(5, user);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task DeleteUser_ShouldReturnNoContent()
        {
            _userServiceMock.Setup(s => s.DeleteUserAsync(5)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteUser(5);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }
    }
}
