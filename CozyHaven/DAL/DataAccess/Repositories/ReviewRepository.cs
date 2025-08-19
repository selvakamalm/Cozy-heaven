using DAL.Context;
using DAL.DataAccess.Interface;
using DAL.Models.Main;
using Microsoft.EntityFrameworkCore;

namespace DAL.DataAccess.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly CozyHavenDbContext _context;

        public ReviewRepository(CozyHavenDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetAllAsync() =>
            await _context.Reviews.Include(r => r.Hotel).Include(r => r.User).ToListAsync();

        public async Task<Review?> GetByIdAsync(int id) =>
            await _context.Reviews.FirstOrDefaultAsync(r => r.Id == id);

        public async Task AddAsync(Review entity) => await _context.Reviews.AddAsync(entity);

        public void Update(Review entity) => _context.Reviews.Update(entity);

        public void Delete(Review entity) => _context.Reviews.Remove(entity);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<IEnumerable<Review>> GetReviewsByHotelIdAsync(int hotelId) =>
            await _context.Reviews.Where(r => r.HotelId == hotelId).ToListAsync();
    }
}
