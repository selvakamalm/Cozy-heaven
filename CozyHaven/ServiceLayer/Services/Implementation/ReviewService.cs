using DAL.DataAccess.Interface;
using DAL.Models.Main;
using ServiceLayer.Services.Interfaces;

namespace ServiceLayer.Services.Implementation
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await _reviewRepository.GetAllAsync();
        }

        public async Task<Review?> GetReviewByIdAsync(int id)
        {
            return await _reviewRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Review>> GetReviewsByHotelIdAsync(int hotelId)
        {
            return await _reviewRepository.GetReviewsByHotelIdAsync(hotelId);
        }

        public async Task<Review> AddReviewAsync(Review review)
        {
            await _reviewRepository.AddAsync(review);
            await _reviewRepository.SaveChangesAsync();
            return review;
        }

        public async Task<bool> UpdateReviewAsync(int id, Review updatedReview)
        {
            var existing = await _reviewRepository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.Rating = updatedReview.Rating;
            existing.Comment = updatedReview.Comment;
            existing.CreatedAt = updatedReview.CreatedAt;
            existing.HotelId = updatedReview.HotelId;
            existing.UserId = updatedReview.UserId;

            _reviewRepository.Update(existing);
            await _reviewRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteReviewAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null) return false;

            _reviewRepository.Delete(review);
            await _reviewRepository.SaveChangesAsync();
            return true;
        }
    }
}
