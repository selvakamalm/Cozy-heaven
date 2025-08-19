using DAL.Models.Main;

namespace ServiceLayer.Services.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetAllRoomsAsync();
        Task<Room?> GetRoomByIdAsync(int id);
        Task<IEnumerable<Room>> GetRoomsByHotelIdAsync(int hotelId);
        Task<Room> AddRoomAsync(Room room);
        Task<bool> UpdateRoomAsync(int id, Room updatedRoom);
        Task<bool> DeleteRoomAsync(int id);
    }
}
