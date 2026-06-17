
   namespace ReservationHotel.DTOs
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string RoomType { get; set; } = "";
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        
    }
}
