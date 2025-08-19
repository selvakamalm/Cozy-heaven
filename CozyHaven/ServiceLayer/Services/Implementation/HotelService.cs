using DAL.DataAccess.Interface;
using DAL.Models.Main;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Services.Interfaces;

namespace ServiceLayer.Services.Implementation
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;

        public HotelService(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<IEnumerable<Hotel>> GetAllHotelsAsync()
        {
            return await _hotelRepository.GetAllAsync();
        }

        public async Task<Hotel?> GetHotelByIdAsync(int id)
        {
            return await _hotelRepository.GetByIdAsync(id);
        }

        public async Task<Hotel> AddHotelAsync(Hotel hotel)
        {
            await _hotelRepository.AddAsync(hotel);
            await _hotelRepository.SaveChangesAsync();
            return hotel;
        }

        public async Task<bool> UpdateHotelAsync(int id, Hotel updatedHotel)
        {
            var existing = await _hotelRepository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.Name = updatedHotel.Name;
            existing.Location = updatedHotel.Location;
            existing.Description = updatedHotel.Description;
            existing.OwnerId = updatedHotel.OwnerId;

            _hotelRepository.Update(existing);
            await _hotelRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteHotelAsync(int id)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id);
            if (hotel == null) return false;

            _hotelRepository.Delete(hotel);
            await _hotelRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Hotel>> GetHotelsByLocationAsync(string location)
        {
            return await _hotelRepository.GetHotelsByLocationAsync(location);
        }

        public async Task<IEnumerable<Hotel>> GetHotelsByOwnerAsync(int ownerId)
        {
            return await _hotelRepository.GetHotelsByOwnerAsync(ownerId);
        }
        //public async Task<IEnumerable<Hotel>> GetHotelsByOwnerIdAsync(int ownerId)
        //{
        //    return await _hotelRepository.GetHotelsByOwnerAsync(ownerId);
        //}

        public async Task<Hotel?> GetHotelByNameAsync(string name)
        {
            return await _hotelRepository.GetHotelByNameAsync(name);
        }




    }
}
