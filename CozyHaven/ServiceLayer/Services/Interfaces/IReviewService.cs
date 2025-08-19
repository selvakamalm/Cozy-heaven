using DAL.Models.Main;

namespace ServiceLayer.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<Review?> GetReviewByIdAsync(int id);
        Task<IEnumerable<Review>> GetReviewsByHotelIdAsync(int hotelId);
        Task<Review> AddReviewAsync(Review review);
        Task<bool> UpdateReviewAsync(int id, Review review);
        Task<bool> DeleteReviewAsync(int id);
    }
}
