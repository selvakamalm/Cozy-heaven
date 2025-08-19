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
    public class Hotel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Location { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [ForeignKey("Owner")]
        public int OwnerId { get; set; }

        // Navigation Properties
        [JsonIgnore]
        [ValidateNever]
        public User Owner { get; set; }

        public ICollection<Room> Rooms { get; set; } = new List<Room>();

        public ICollection<HotelFacility> Facilities { get; set; } = new List<HotelFacility>();

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }

}
