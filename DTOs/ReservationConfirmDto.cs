namespace ReservationHotel.DTOs
{
    public class ReservationConfirmDto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string RoomType { get; set; } = "";
        public string HotelName { get; set; } = "";     // ← Ajoute cette ligne
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int Guests { get; set; }
        public int Nights { get; set; }
        public decimal TotalPrice { get; set; }
    }
}