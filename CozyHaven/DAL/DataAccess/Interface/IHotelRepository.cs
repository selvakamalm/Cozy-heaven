using DAL.Models.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataAccess.Interface
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        Task<IEnumerable<Hotel>> GetHotelsByLocationAsync(string location);
        Task<IEnumerable<Hotel>> GetHotelsByOwnerAsync(int ownerId);
        Task<Hotel?> GetHotelByNameAsync(string name);


    }
}
