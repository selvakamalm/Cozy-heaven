using DAL.DataAccess.Interface;
using DAL.Models;
using ServiceLayer.Services.Interfaces;

namespace ServiceLayer.Services.Implementation
{
    public class FacilityService : IFacilityService
    {
        private readonly IFacilityRepository _facilityRepository;

        public FacilityService(IFacilityRepository facilityRepository)
        {
            _facilityRepository = facilityRepository;
        }

        public async Task<IEnumerable<Facility>> GetAllFacilitiesAsync()
        {
            return await _facilityRepository.GetAllAsync();
        }

        public async Task<Facility?> GetFacilityByIdAsync(int id)
        {
            return await _facilityRepository.GetByIdAsync(id);
        }

        public async Task AddFacilityAsync(Facility facility)
        {
            await _facilityRepository.AddAsync(facility);
            await _facilityRepository.SaveChangesAsync();
        }

        public async Task UpdateFacilityAsync(Facility facility)
        {
            _facilityRepository.Update(facility);
            await _facilityRepository.SaveChangesAsync();
        }

        public async Task DeleteFacilityAsync(int id)
        {
            var facility = await _facilityRepository.GetByIdAsync(id);
            if (facility != null)
            {
                _facilityRepository.Delete(facility);
                await _facilityRepository.SaveChangesAsync();
            }
        }
    }
}
