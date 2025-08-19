using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services.Interfaces;

namespace ServiceLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacilityController : ControllerBase
    {
        private readonly IFacilityService _facilityService;

        public FacilityController(IFacilityService facilityService)
        {
            _facilityService = facilityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var facilities = await _facilityService.GetAllFacilitiesAsync();
            return Ok(facilities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var facility = await _facilityService.GetFacilityByIdAsync(id);
            if (facility == null)
                return NotFound();
            return Ok(facility);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Facility facility)
        {
            await _facilityService.AddFacilityAsync(facility);
            return CreatedAtAction(nameof(GetById), new { id = facility.Id }, facility);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Facility facility)
        {
            if (id != facility.Id)
                return BadRequest();

            await _facilityService.UpdateFacilityAsync(facility);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _facilityService.DeleteFacilityAsync(id);
            return NoContent();
        }
    }
}
