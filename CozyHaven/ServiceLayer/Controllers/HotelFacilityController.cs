using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services.Interfaces;

namespace ServiceLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelFacilityController : ControllerBase
    {
        private readonly IHotelFacilityService _hotelFacilityService;

        public HotelFacilityController(IHotelFacilityService hotelFacilityService)
        {
            _hotelFacilityService = hotelFacilityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _hotelFacilityService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{hotelId:int}/{facilityId:int}")]
        public async Task<IActionResult> Get(int hotelId, int facilityId)
        {
            var result = await _hotelFacilityService.GetByIdAsync(hotelId, facilityId);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] HotelFacility model)
        {
            await _hotelFacilityService.AddAsync(model);
            return Ok(model);
        }

        [HttpDelete("{hotelId:int}/{facilityId:int}")]
        public async Task<IActionResult> Delete(int hotelId, int facilityId)
        {
            await _hotelFacilityService.DeleteAsync(hotelId, facilityId);
            return NoContent();
        }

        [HttpGet("hotel/{hotelId:int}")]
        public async Task<IActionResult> GetFacilitiesForHotel(int hotelId)
        {
            var facilities = await _hotelFacilityService.GetFacilitiesForHotelAsync(hotelId);
            return Ok(facilities);
        }
    }
}
