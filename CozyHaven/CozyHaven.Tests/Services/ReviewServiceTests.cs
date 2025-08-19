using DAL.DataAccess.Interface;
using DAL.Models.Main;
using Moq;
using NUnit.Framework.Legacy;
using ServiceLayer.Services.Implementation;

namespace CozyHaven.Tests.Services
{
    [TestFixture]
    public class ReviewServiceTests
    {
        private Mock<IReviewRepository> _reviewRepoMock = null!;
        private ReviewService _reviewService = null!;

        [SetUp]
        public void SetUp()
        {
            _reviewRepoMock = new Mock<IReviewRepository>();
            _reviewService = new ReviewService(_reviewRepoMock.Object);
        }

        [Test]
        public async Task GetAllReviewsAsync_ShouldReturnList()
        {
            var fakeReviews = new List<Review>
            {
                new Review { Id = 1 },
                new Review { Id = 2 }
            };

            _reviewRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(fakeReviews);

            var result = await _reviewService.GetAllReviewsAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetReviewByIdAsync_ShouldReturnReview_WhenExists()
        {
            var review = new Review { Id = 1 };

            _reviewRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(review);

            var result = await _reviewService.GetReviewByIdAsync(1);

            Assert.That(result, Is.EqualTo(review));
        }

        [Test]
        public async Task GetReviewByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            _reviewRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Review?)null);

            var result = await _reviewService.GetReviewByIdAsync(99);

            ClassicAssert.IsNull(result);
        }

        [Test]
        public async Task GetReviewsByHotelIdAsync_ShouldReturnHotelReviews()
        {
            var reviews = new List<Review> { new Review { HotelId = 5 } };

            _reviewRepoMock.Setup(r => r.GetReviewsByHotelIdAsync(5)).ReturnsAsync(reviews);

            var result = await _reviewService.GetReviewsByHotelIdAsync(5);

            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task AddReviewAsync_ShouldReturnCreatedReview()
        {
            var review = new Review { Id = 1, Rating = 5 };

            _reviewRepoMock.Setup(r => r.AddAsync(review)).Returns(Task.CompletedTask);
            _reviewRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _reviewService.AddReviewAsync(review);

            Assert.That(result, Is.EqualTo(review));
            _reviewRepoMock.Verify(r => r.AddAsync(review), Times.Once);
            _reviewRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateReviewAsync_ShouldReturnFalse_WhenNotFound()
        {
            _reviewRepoMock.Setup(r => r.GetByIdAsync(10)).ReturnsAsync((Review?)null);

            var result = await _reviewService.UpdateReviewAsync(10, new Review());

            ClassicAssert.IsFalse(result);
        }

        [Test]
        public async Task UpdateReviewAsync_ShouldReturnTrue_WhenSuccess()
        {
            var existing = new Review { Id = 1 };
            var updated = new Review
            {
                Id = 1,
                Rating = 4,
                Comment = "Nice",
                CreatedAt = DateTime.Now,
                HotelId = 2,
                UserId = 3
            };

            _reviewRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _reviewRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _reviewService.UpdateReviewAsync(1, updated);

            ClassicAssert.IsTrue(result);
            _reviewRepoMock.Verify(r => r.Update(It.Is<Review>(r =>
                r.Rating == 4 &&
                r.Comment == "Nice" &&
                r.HotelId == 2 &&
                r.UserId == 3
            )), Times.Once);
        }

        [Test]
        public async Task DeleteReviewAsync_ShouldReturnFalse_WhenNotFound()
        {
            _reviewRepoMock.Setup(r => r.GetByIdAsync(50)).ReturnsAsync((Review?)null);

            var result = await _reviewService.DeleteReviewAsync(50);

            ClassicAssert.IsFalse(result);
        }

        [Test]
        public async Task DeleteReviewAsync_ShouldReturnTrue_WhenSuccess()
        {
            var review = new Review { Id = 1 };

            _reviewRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(review);
            _reviewRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _reviewService.DeleteReviewAsync(1);

            ClassicAssert.IsTrue(result);
            _reviewRepoMock.Verify(r => r.Delete(review), Times.Once);
        }
    }
}
