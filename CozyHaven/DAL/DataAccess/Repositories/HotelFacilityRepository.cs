using DAL.Context;
using DAL.DataAccess.Interface;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataAccess.Repositories
{
    public class HotelFacilityRepository : IHotelFacilityRepository
    {
        private readonly CozyHavenDbContext _context;

        public HotelFacilityRepository(CozyHavenDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HotelFacility>> GetAllAsync()
        {
            return await _context.HotelFacilities
                .Include(hf => hf.Hotel)
                .Include(hf => hf.Facility)
                .ToListAsync();
        }

        public async Task<HotelFacility?> GetByIdAsync(int hotelId, int facilityId)
        {
            return await _context.HotelFacilities
                .FirstOrDefaultAsync(hf => hf.HotelId == hotelId && hf.FacilityId == facilityId);
        }

        public async Task AddAsync(HotelFacility entity)
        {
            await _context.HotelFacilities.AddAsync(entity);
        }

        public void Update(HotelFacility entity)
        {
            _context.HotelFacilities.Update(entity);
        }

        public void Delete(HotelFacility entity)
        {
            _context.HotelFacilities.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Facility>> GetFacilitiesForHotelAsync(int hotelId)
        {
            return await _context.HotelFacilities
                .Where(hf => hf.HotelId == hotelId)
                .Include(hf => hf.Facility)
                .Select(hf => hf.Facility)
                .ToListAsync();
        }
    }
}
