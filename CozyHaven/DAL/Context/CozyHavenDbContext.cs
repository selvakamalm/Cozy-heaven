using DAL.Models;
using DAL.Models.Main;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public class CozyHavenDbContext : DbContext
    {
        public CozyHavenDbContext(DbContextOptions<CozyHavenDbContext> options)
            : base(options)
        {
        }

        // DbSet Properties to respesnt data in table
        public DbSet<User> Users { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<HotelFacility> HotelFacilities { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomAvailability> RoomAvailabilities { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // HotelFacility many-to-many
            modelBuilder.Entity<HotelFacility>().HasKey(hf => new { hf.HotelId, hf.FacilityId });

            modelBuilder.Entity<HotelFacility>()
                .HasOne(hf => hf.Hotel)
                .WithMany(h => h.Facilities)
                .HasForeignKey(hf => hf.HotelId);

            modelBuilder.Entity<HotelFacility>()
                .HasOne(hf => hf.Facility)
                .WithMany()
                .HasForeignKey(hf => hf.FacilityId);

            // Room - Hotel
            modelBuilder.Entity<Room>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Rooms)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            // RoomAvailability - Room
            modelBuilder.Entity<RoomAvailability>()
                .HasOne(ra => ra.Room)
                .WithMany(r => r.Availabilities)
                .HasForeignKey(ra => ra.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            // Booking - User
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict); // prevent multiple cascade paths

            // Booking - Room
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Room)
                .WithMany()
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            // Payment - Booking
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithMany(b => b.Payments)
                .HasForeignKey(p => p.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Review - User
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict); // prevent multiple cascade paths

            // Review - Hotel
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Reviews)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Booking - Hotel
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Hotel)
                .WithMany(h => h.Bookings)
                .HasForeignKey(b => b.HotelId)
                .OnDelete(DeleteBehavior.Restrict); // prevent cascade path conflict
        }
    }
}
