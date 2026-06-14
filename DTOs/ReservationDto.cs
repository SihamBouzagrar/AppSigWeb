namespace ReservationHotel.DTOs
{
    // CS8618 FIX : propriétés string initialisées à "" pour éviter les warnings nullable
    public class ReservationDto
    {
        public int RoomId { get; set; }
 
        public string FullName { get; set; } = "";
 
        public string Email { get; set; } = "";
 
        public string Phone { get; set; } = "";
 
        public DateTime CheckIn { get; set; }
 
        public DateTime CheckOut { get; set; }
    }
}
 
