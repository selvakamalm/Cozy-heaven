using DAL.Models;

namespace ServiceLayer.Services.Interfaces
{
    public interface IHotelFacilityService
    {
        Task<IEnumerable<HotelFacility>> GetAllAsync();
        Task<HotelFacility?> GetByIdAsync(int hotelId, int facilityId);
        Task AddAsync(HotelFacility hotelFacility);
        Task DeleteAsync(int hotelId, int facilityId);
        Task<IEnumerable<Facility>> GetFacilitiesForHotelAsync(int hotelId);
    }
}
