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
    public class BookingRepository : IBookingRepository
    {
        private readonly CozyHavenDbContext _context;
        //
        public BookingRepository(CozyHavenDbContext context)
        {
            _context = context;
        }

        //fetches all the booking
        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .Include(b => b.Hotel)
                .ToListAsync();
        }
        // to use async  to do long running task without freezing
        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .Include(b => b.Hotel) // ✅ include Hotel
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task AddAsync(Booking entity)
        {
            await _context.Bookings.AddAsync(entity);
        }

        public void Update(Booking entity)
        {
            _context.Bookings.Update(entity);
        }

        public void Delete(Booking entity)
        {
            _context.Bookings.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId)
        {
            return await _context.Bookings
                .Include(b => b.Room)
                 .Include(b => b.Hotel) // ✅ include Hotel
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }
    }
}
