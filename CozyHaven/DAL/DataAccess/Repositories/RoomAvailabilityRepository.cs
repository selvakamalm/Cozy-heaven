using DAL.Context;
using DAL.DataAccess.Interface;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.DataAccess.Repositories
{
    public class RoomAvailabilityRepository : IRoomAvailabilityRepository
    {
        private readonly CozyHavenDbContext _context;

        public RoomAvailabilityRepository(CozyHavenDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RoomAvailability>> GetAvailableRoomsAsync(int roomId, DateTime fromDate, DateTime toDate)
        {
            return await _context.RoomAvailabilities
                .Where(ra => ra.RoomId == roomId &&
                             ra.IsAvailable &&
                             ra.Date >= fromDate &&
                             ra.Date <= toDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<RoomAvailability>> GetAllAsync()
        {
            return await _context.RoomAvailabilities.ToListAsync();
        }

        public async Task<RoomAvailability?> GetByIdAsync(int id)
        {
            return await _context.RoomAvailabilities.FindAsync(id);
        }

        public async Task AddAsync(RoomAvailability entity)
        {
            await _context.RoomAvailabilities.AddAsync(entity);
        }

        public void Update(RoomAvailability entity)
        {
            _context.RoomAvailabilities.Update(entity);
        }

        public void Delete(RoomAvailability entity)
        {
            _context.RoomAvailabilities.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

