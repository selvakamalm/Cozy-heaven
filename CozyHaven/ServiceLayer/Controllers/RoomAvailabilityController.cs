using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services.Interfaces;

namespace ServiceLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomAvailabilityController : ControllerBase
    {
        private readonly IRoomAvailabilityService _availabilityService;

        public RoomAvailabilityController(IRoomAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _availabilityService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var availability = await _availabilityService.GetByIdAsync(id);
            if (availability == null) return NotFound();
            return Ok(availability);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableRooms(int roomId, DateTime from, DateTime to)
        {
            var available = await _availabilityService.GetAvailableRoomsAsync(roomId, from, to);
            return Ok(available);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] RoomAvailability availability)
        {
            var result = await _availabilityService.AddAsync(availability);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RoomAvailability availability)
        {
            if (id != availability.Id) return BadRequest();

            var updated = await _availabilityService.UpdateAsync(availability);
            if (!updated) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _availabilityService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
