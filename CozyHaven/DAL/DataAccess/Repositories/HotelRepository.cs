using DAL.Context;
using DAL.DataAccess.Interface;
using DAL.Models.Main;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataAccess.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly CozyHavenDbContext _context;

        public HotelRepository(CozyHavenDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Hotel>> GetAllAsync()
        {
            return await _context.Hotels
                .Include(h => h.Rooms)
                .Include(h => h.Reviews)
                .ToListAsync();
        }

        public async Task<Hotel?> GetByIdAsync(int id)
        {
            return await _context.Hotels
                .Include(h => h.Rooms)
                .Include(h => h.Reviews)
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task AddAsync(Hotel hotel)
        {
            await _context.Hotels.AddAsync(hotel);
        }

        public void Update(Hotel hotel)
        {
            _context.Hotels.Update(hotel);
        }

        public void Delete(Hotel hotel)
        {
            _context.Hotels.Remove(hotel);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Hotel>> GetHotelsByOwnerAsync(int ownerId)
        {
            return await _context.Hotels
                .Include(h => h.Rooms)
                .Where(h => h.OwnerId == ownerId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Hotel>> GetHotelsByLocationAsync(string location)
        {
            return await _context.Hotels
                .Where(h => h.Location.Contains(location))
                .ToListAsync();
        }

        public async Task<Hotel?> GetHotelByNameAsync(string name)
        {
            return await _context.Hotels.FirstOrDefaultAsync(h => h.Name == name);
        }



    }
}
