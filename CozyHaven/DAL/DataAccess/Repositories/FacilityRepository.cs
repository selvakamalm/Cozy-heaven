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
    public class FacilityRepository : IFacilityRepository
    {
        private readonly CozyHavenDbContext _context;

        public FacilityRepository(CozyHavenDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Facility>> GetAllAsync()
        {
            return await _context.Facilities.ToListAsync();
        }

        public async Task<Facility?> GetByIdAsync(int id)
        {
            return await _context.Facilities.FindAsync(id);
        }

        public async Task AddAsync(Facility entity)
        {
            await _context.Facilities.AddAsync(entity);
        }

        public void Update(Facility entity)
        {
            _context.Facilities.Update(entity);
        }

        public void Delete(Facility entity)
        {
            _context.Facilities.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
