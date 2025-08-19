using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataAccess.Interface
{
    public interface IHotelFacilityRepository 
    {
        Task<IEnumerable<HotelFacility>> GetAllAsync();
        Task<HotelFacility?> GetByIdAsync(int hotelId, int facilityId);
        Task AddAsync(HotelFacility entity);
        void Update(HotelFacility entity);
        void Delete(HotelFacility entity);
        Task SaveChangesAsync();

        Task<IEnumerable<Facility>> GetFacilitiesForHotelAsync(int hotelId);
        //Task<IEnumerable<Hotel>> GetHotelsForFacilityAsync(int facilityId);
    }
}
