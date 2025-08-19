using DAL.DataAccess.Interface;
using DAL.Models.Main;
using ServiceLayer.Services.Interfaces;

namespace ServiceLayer.Services.Implementation
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            return await _roomRepository.GetAllAsync();
        }

        public async Task<Room?> GetRoomByIdAsync(int id)
        {
            return await _roomRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Room>> GetRoomsByHotelIdAsync(int hotelId)
        {
            return await _roomRepository.GetRoomsByHotelIdAsync(hotelId);
        }

        public async Task<Room> AddRoomAsync(Room room)
        {
            await _roomRepository.AddAsync(room);
            await _roomRepository.SaveChangesAsync();
            return room;
        }

        public async Task<bool> UpdateRoomAsync(int id, Room updatedRoom)
        {
            var existing = await _roomRepository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.HotelId = updatedRoom.HotelId;
            existing.RoomSize = updatedRoom.RoomSize;
            existing.BedType = updatedRoom.BedType;
            existing.MaxOccupancy = updatedRoom.MaxOccupancy;
            existing.BaseFare = updatedRoom.BaseFare;
            existing.IsAC = updatedRoom.IsAC;

            _roomRepository.Update(existing);
            await _roomRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRoomAsync(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null) return false;

            _roomRepository.Delete(room);
            await _roomRepository.SaveChangesAsync();
            return true;
        }
    }
}
