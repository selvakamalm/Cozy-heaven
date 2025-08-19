using DAL.DataAccess.Interface;
using DAL.Models;
using ServiceLayer.Services.Interfaces;

namespace ServiceLayer.Services.Implementation
{
    public class HotelFacilityService : IHotelFacilityService
    {
        private readonly IHotelFacilityRepository _hotelFacilityRepository;

        public HotelFacilityService(IHotelFacilityRepository hotelFacilityRepository)
        {
            _hotelFacilityRepository = hotelFacilityRepository;
        }

        public async Task<IEnumerable<HotelFacility>> GetAllAsync()
        {
            return await _hotelFacilityRepository.GetAllAsync();
        }

        public async Task<HotelFacility?> GetByIdAsync(int hotelId, int facilityId)
        {
            return await _hotelFacilityRepository.GetByIdAsync(hotelId, facilityId);
        }

        public async Task AddAsync(HotelFacility hotelFacility)
        {
            await _hotelFacilityRepository.AddAsync(hotelFacility);
            await _hotelFacilityRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int hotelId, int facilityId)
        {
            var entity = await _hotelFacilityRepository.GetByIdAsync(hotelId, facilityId);
            if (entity != null)
            {
                _hotelFacilityRepository.Delete(entity);
                await _hotelFacilityRepository.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Facility>> GetFacilitiesForHotelAsync(int hotelId)
        {
            return await _hotelFacilityRepository.GetFacilitiesForHotelAsync(hotelId);
        }
    }
}
