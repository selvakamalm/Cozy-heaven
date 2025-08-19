using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Models.Main
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Hotel")]
        public int HotelId { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoomSize { get; set; } // e.g., 70m^2

        [Required]
        [MaxLength(30)]
        public string BedType { get; set; } // Single, Double, King

        [Required]
        [Range(1, 10)]
        public int MaxOccupancy { get; set; }

        [Required]
        [Range(0, 100000)]
        public decimal BaseFare { get; set; }

        [Required]
        public bool IsAC { get; set; }

        // Navigation properties
        [JsonIgnore]
        [ValidateNever]
        public Hotel Hotel { get; set; }
        public ICollection<RoomAvailability> Availabilities { get; set; } = new List<RoomAvailability>();
    }

}
