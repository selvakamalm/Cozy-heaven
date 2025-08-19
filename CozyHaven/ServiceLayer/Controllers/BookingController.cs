using DAL.Models.Main;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services;
using ServiceLayer.Services.Implementation;
using ServiceLayer.Services.Interfaces;

namespace ServiceLayer.Controllers
{    
    //https:/localhost:7298/api/booking
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IHotelService _hotelService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // GET: api/booking
        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }

        // GET: api/booking/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
                return NotFound();

            return Ok(booking);
        }

        // GET: api/booking/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBookingsByUserId(int userId)
        {
            var bookings = await _bookingService.GetBookingsByUserIdAsync(userId);
            return Ok(bookings);
        }

        // POST: api/booking
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] Booking booking)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdBooking = await _bookingService.AddBookingAsync(booking);
            return CreatedAtAction(nameof(GetBookingById), new { id = createdBooking.Id }, createdBooking);
        }

        // PUT: api/booking/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] Booking booking)
        {
            if (id != booking.Id)
                return BadRequest("Booking ID mismatch");

            var updated = await _bookingService.UpdateBookingAsync(id, booking);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/booking/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var deleted = await _bookingService.DeleteBookingAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpPost("book-by-hotel-name")]
        public async Task<IActionResult> BookByHotelName([FromBody] BookingByHotelNameRequest request)
        {
            // 1. Lookup hotel by name
            var hotel = await _hotelService.GetHotelByNameAsync(request.HotelName);

            if (hotel == null)
                return NotFound($"Hotel '{request.HotelName}' not found");

            // 2. Create booking entity
            var booking = new Booking
            {
                HotelId = hotel.Id,
                RoomId = request.RoomId,
                UserId = request.UserId,
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                NumberOfGuests = request.NumberOfGuests,
                TotalAmount = request.TotalAmount,
                Status = "Pending"
            };

            // 3. Save booking
            var result = await _bookingService.AddBookingAsync(booking);
            return CreatedAtAction(nameof(GetBookingById), new { id = result.Id }, result);
        }

    }
}
