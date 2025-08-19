namespace ServiceLayer.Services
{
    public class BookingByHotelNameRequest
    {
        public string HotelName { get; set; } = string.Empty;
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
        public decimal TotalAmount { get; set; }
    }

}
