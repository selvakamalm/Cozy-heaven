using DAL.Models.Main;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services.Interfaces;

namespace ServiceLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Hotel Owner,User")]

    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        // GET: api/hotel
        [HttpGet]
        public async Task<IActionResult> GetAllHotels()
        {
            var hotels = await _hotelService.GetAllHotelsAsync();
            return Ok(hotels);
        }

        // GET: api/hotel/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotelById(int id)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);
            if (hotel == null)
                return NotFound();

            return Ok(hotel);
        }

        // GET: api/hotel/location?location=Delhi
        [HttpGet("location")]
        public async Task<IActionResult> GetHotelsByLocation([FromQuery] string location)
        {
            var hotels = await _hotelService.GetHotelsByLocationAsync(location);
            return Ok(hotels);
        }

        // POST: api/hotel
        [HttpPost]
        public async Task<IActionResult> CreateHotel([FromBody] Hotel hotel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdHotel = await _hotelService.AddHotelAsync(hotel);
            return CreatedAtAction(nameof(GetHotelById), new { id = createdHotel.Id }, createdHotel);
        }

        // PUT: api/hotel/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] Hotel hotel)
        {
            if (id != hotel.Id)
                return BadRequest("Hotel ID mismatch");

            var updated = await _hotelService.UpdateHotelAsync(id, hotel);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/hotel/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var deleted = await _hotelService.DeleteHotelAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        // GET: api/hotel/owner/4
        [HttpGet("owner/{ownerId}")]
        public async Task<IActionResult> GetHotelsByOwnerId(int ownerId)
        {
            var hotels = await _hotelService.GetHotelsByOwnerAsync(ownerId);
            if (hotels == null || !hotels.Any())
                return NotFound();

            return Ok(hotels);
        }

    }
}
