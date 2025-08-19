using DAL.Models.Main;

namespace ServiceLayer.Services.Interfaces
{
    public interface IHotelService
    {
        Task<IEnumerable<Hotel>> GetAllHotelsAsync();
        Task<Hotel?> GetHotelByIdAsync(int id);
        Task<Hotel> AddHotelAsync(Hotel hotel);
        Task<bool> UpdateHotelAsync(int id, Hotel updatedHotel);
        Task<bool> DeleteHotelAsync(int id);
        Task<IEnumerable<Hotel>> GetHotelsByLocationAsync(string location);
        Task<IEnumerable<Hotel>> GetHotelsByOwnerAsync(int ownerId);

        Task<Hotel?> GetHotelByNameAsync(string name);



        //Task<IEnumerable<Hotel>> GetHotelsByOwnerAsync(int ownerId);
    }
}
