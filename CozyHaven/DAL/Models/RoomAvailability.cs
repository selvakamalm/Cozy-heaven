using DAL.Models.Main;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Models
{
    //per-day availability without modifying the Room data.
    public class RoomAvailability
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public DateTime Date { get; set; }
        public bool IsAvailable { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public Room Room { get; set; }
    }

}
