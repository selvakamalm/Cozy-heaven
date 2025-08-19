using DAL.Models.Main;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ServiceLayer.Controllers;
using ServiceLayer.Services.Interfaces;

namespace CozyHaven.Tests.Controllers
{
    [TestFixture]
    public class ReviewControllerTests
    {
        private Mock<IReviewService> _reviewServiceMock = null!;
        private ReviewController _reviewController = null!;

        [SetUp]
        public void SetUp()
        {
            _reviewServiceMock = new Mock<IReviewService>();
            _reviewController = new ReviewController(_reviewServiceMock.Object);
        }

        [Test]
        public async Task GetAllReviews_ShouldReturnOkWithReviews()
        {
            var fakeReviews = new List<Review> { new Review { Id = 1 }, new Review { Id = 2 } };
            _reviewServiceMock.Setup(s => s.GetAllReviewsAsync()).ReturnsAsync(fakeReviews);

            var result = await _reviewController.GetAllReviews();

            var ok = result as OkObjectResult;
            Assert.That(ok, Is.Not.Null);
            Assert.That(ok!.Value, Is.EqualTo(fakeReviews));
        }

        [Test]
        public async Task GetReviewById_ShouldReturnOk_WhenExists()
        {
            var review = new Review { Id = 1 };
            _reviewServiceMock.Setup(s => s.GetReviewByIdAsync(1)).ReturnsAsync(review);

            var result = await _reviewController.GetReviewById(1);

            var ok = result as OkObjectResult;
            Assert.That(ok, Is.Not.Null);
            Assert.That(ok!.Value, Is.EqualTo(review));
        }

        [Test]
        public async Task GetReviewById_ShouldReturnNotFound_WhenMissing()
        {
            _reviewServiceMock.Setup(s => s.GetReviewByIdAsync(100)).ReturnsAsync((Review?)null);

            var result = await _reviewController.GetReviewById(100);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetReviewsByHotelId_ShouldReturnOk()
        {
            var reviews = new List<Review> { new Review { HotelId = 1 }, new Review { HotelId = 1 } };
            _reviewServiceMock.Setup(s => s.GetReviewsByHotelIdAsync(1)).ReturnsAsync(reviews);

            var result = await _reviewController.GetReviewsByHotelId(1);

            var ok = result as OkObjectResult;
            Assert.That(ok, Is.Not.Null);
            Assert.That(ok!.Value, Is.EqualTo(reviews));
        }

        [Test]
        public async Task AddReview_ShouldReturnCreatedAtAction()
        {
            var review = new Review { Id = 1 };
            _reviewServiceMock.Setup(s => s.AddReviewAsync(review)).ReturnsAsync(review);

            var result = await _reviewController.AddReview(review);

            var created = result as CreatedAtActionResult;
            Assert.That(created, Is.Not.Null);
            Assert.That(created!.ActionName, Is.EqualTo(nameof(_reviewController.GetReviewById)));
            Assert.That(created.Value, Is.EqualTo(review));
        }

        [Test]
        public async Task UpdateReview_ShouldReturnNoContent_WhenSuccess()
        {
            var review = new Review { Id = 1 };
            _reviewServiceMock.Setup(s => s.UpdateReviewAsync(1, review)).ReturnsAsync(true);

            var result = await _reviewController.UpdateReview(1, review);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task UpdateReview_ShouldReturnNotFound_WhenFails()
        {
            var review = new Review { Id = 1 };
            _reviewServiceMock.Setup(s => s.UpdateReviewAsync(1, review)).ReturnsAsync(false);

            var result = await _reviewController.UpdateReview(1, review);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteReview_ShouldReturnNoContent_WhenSuccess()
        {
            _reviewServiceMock.Setup(s => s.DeleteReviewAsync(1)).ReturnsAsync(true);

            var result = await _reviewController.DeleteReview(1);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteReview_ShouldReturnNotFound_WhenFails()
        {
            _reviewServiceMock.Setup(s => s.DeleteReviewAsync(99)).ReturnsAsync(false);

            var result = await _reviewController.DeleteReview(99);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
