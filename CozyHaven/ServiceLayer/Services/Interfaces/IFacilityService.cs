using DAL.Models;

namespace ServiceLayer.Services.Interfaces
{
    public interface IFacilityService
    {
        Task<IEnumerable<Facility>> GetAllFacilitiesAsync();
        Task<Facility?> GetFacilityByIdAsync(int id);
        Task AddFacilityAsync(Facility facility);
        Task UpdateFacilityAsync(Facility facility);
        Task DeleteFacilityAsync(int id);
    }
}
