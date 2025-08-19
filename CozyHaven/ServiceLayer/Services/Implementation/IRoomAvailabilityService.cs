using DAL.DataAccess.Interface;
using DAL.Models;
using ServiceLayer.Services.Interfaces;

namespace ServiceLayer.Services.Implementation
{
    public class RoomAvailabilityService : IRoomAvailabilityService
    {
        private readonly IRoomAvailabilityRepository _availabilityRepo;

        public RoomAvailabilityService(IRoomAvailabilityRepository availabilityRepo)
        {
            _availabilityRepo = availabilityRepo;
        }

        public async Task<IEnumerable<RoomAvailability>> GetAllAsync()
        {
            return await _availabilityRepo.GetAllAsync();
        }

        public async Task<RoomAvailability?> GetByIdAsync(int id)
        {
            return await _availabilityRepo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<RoomAvailability>> GetAvailableRoomsAsync(int roomId, DateTime fromDate, DateTime toDate)
        {
            return await _availabilityRepo.GetAvailableRoomsAsync(roomId, fromDate, toDate);
        }

        public async Task<RoomAvailability> AddAsync(RoomAvailability availability)
        {
            await _availabilityRepo.AddAsync(availability);
            await _availabilityRepo.SaveChangesAsync();
            return availability;
        }

        public async Task<bool> UpdateAsync(RoomAvailability availability)
        {
            var existing = await _availabilityRepo.GetByIdAsync(availability.Id);
            if (existing == null) return false;

            existing.Date = availability.Date;
            existing.IsAvailable = availability.IsAvailable;
            existing.RoomId = availability.RoomId;

            _availabilityRepo.Update(existing);
            await _availabilityRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _availabilityRepo.GetByIdAsync(id);
            if (existing == null) return false;

            _availabilityRepo.Delete(existing);
            await _availabilityRepo.SaveChangesAsync();
            return true;
        }
    }
}
