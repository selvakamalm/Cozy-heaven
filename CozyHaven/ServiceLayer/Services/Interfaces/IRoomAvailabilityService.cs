using DAL.Models;

namespace ServiceLayer.Services.Interfaces
{
    public interface IRoomAvailabilityService
    {
        Task<IEnumerable<RoomAvailability>> GetAllAsync();
        Task<RoomAvailability?> GetByIdAsync(int id);
        Task<IEnumerable<RoomAvailability>> GetAvailableRoomsAsync(int roomId, DateTime fromDate, DateTime toDate);
        Task<RoomAvailability> AddAsync(RoomAvailability availability);
        Task<bool> UpdateAsync(RoomAvailability availability);
        Task<bool> DeleteAsync(int id);
    }
}
