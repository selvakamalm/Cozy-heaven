using DAL.Context;
using DAL.DataAccess.Interface;
using DAL.Models.Main;
using Microsoft.EntityFrameworkCore;


namespace DAL.DataAccess.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly CozyHavenDbContext _context;

        public RoomRepository(CozyHavenDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await _context.Rooms
                .Include(r => r.Hotel)
                .ToListAsync();
        }

        public async Task<Room?> GetByIdAsync(int id)
        {
            return await _context.Rooms
                .Include(r => r.Hotel)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddAsync(Room entity)
        {
            await _context.Rooms.AddAsync(entity);
        }

        public void Update(Room entity)
        {
            _context.Rooms.Update(entity);
        }

        public void Delete(Room entity)
        {
            _context.Rooms.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Room>> GetRoomsByHotelIdAsync(int hotelId)
        {
            return await _context.Rooms
                .Where(r => r.HotelId == hotelId)
                .ToListAsync();
        }
    }
}
