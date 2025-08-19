using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Used for Data Validation
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Models.Main
{
    public class Booking
    {
        [Key] // Primary Key  
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")] // Foreign Key for User
        public int UserId { get; set; }

        [Required]
        [ForeignKey("Room")] // Foreign Key for Room
        public int RoomId { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Required]
        [Range(1, 10)]
        public int NumberOfGuests { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } // Booked, Cancelled, Refunded

        // Navigation Properties

        [JsonIgnore]
        [ValidateNever]
        public User User { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public Room Room { get; set; }

       // [JsonIgnore] 
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        [Required]
        public int HotelId { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public Hotel Hotel { get; set; }
    }

}
