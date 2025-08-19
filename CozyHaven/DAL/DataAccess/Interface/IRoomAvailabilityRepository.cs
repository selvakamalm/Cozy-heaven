using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataAccess.Interface
{
    public interface IRoomAvailabilityRepository : IRepository<RoomAvailability>
    {
        Task<IEnumerable<RoomAvailability>> GetAvailableRoomsAsync(int roomId, DateTime fromDate, DateTime toDate);
    }
}
