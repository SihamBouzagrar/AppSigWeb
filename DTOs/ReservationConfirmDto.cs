namespace ReservationHotel.DTOs
{
    public class ReservationConfirmDto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string RoomType { get; set; } = "";
        public string HotelName { get; set; } = "";
        public string? UserFullName { get; set; }   // Pour l'admin
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int Guests { get; set; }
        public int Nights { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Pending";   // ← AJOUTÉ
        
    }
}