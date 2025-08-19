using DAL.DataAccess.Interface;
using DAL.Models.Main;
using ServiceLayer.Services.Interfaces;

namespace ServiceLayer.Services.Implementation
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }
        //setting raw data to the correct place

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();

            return bookings.Select(b => new Booking
            {
                Id = b.Id,
                UserId = b.UserId,
                RoomId = b.RoomId,
                HotelId = b.HotelId,
                CheckInDate = b.CheckInDate,
                CheckOutDate = b.CheckOutDate,
                NumberOfGuests = b.NumberOfGuests,
                TotalAmount = b.TotalAmount,
                Status = b.Status
            });
        }


        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _bookingRepository.GetByIdAsync(id);
        }

        public async Task<Booking> AddBookingAsync(Booking booking)
        {
            await _bookingRepository.AddAsync(booking);
            await _bookingRepository.SaveChangesAsync();
            return booking;
        }

        public async Task<bool> UpdateBookingAsync(int id, Booking updatedBooking)
        {
            var existing = await _bookingRepository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.CheckInDate = updatedBooking.CheckInDate;
            existing.CheckOutDate = updatedBooking.CheckOutDate;
            existing.NumberOfGuests = updatedBooking.NumberOfGuests;
            existing.TotalAmount = updatedBooking.TotalAmount;
            existing.Status = updatedBooking.Status;
            existing.UserId = updatedBooking.UserId;
            existing.RoomId = updatedBooking.RoomId;

            _bookingRepository.Update(existing);
            await _bookingRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBookingAsync(int id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null) return false;

            _bookingRepository.Delete(booking);
            await _bookingRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId)
        {
            return await _bookingRepository.GetBookingsByUserIdAsync(userId);
        }
    }
}
