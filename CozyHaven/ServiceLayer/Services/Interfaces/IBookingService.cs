using DAL.Models.Main;

namespace ServiceLayer.Services.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking?> GetBookingByIdAsync(int id);
        Task<Booking> AddBookingAsync(Booking booking);
        Task<bool> UpdateBookingAsync(int id, Booking updatedBooking);
        Task<bool> DeleteBookingAsync(int id);
        Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId);
       // Task<string?> GetUserBookingsAsync(string? userId);
    }
}
