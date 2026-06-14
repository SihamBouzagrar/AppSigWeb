namespace ReservationHotel.Models
{
    public class Room
    {
        public int Id { get; set; }

        public string RoomType { get; set; } = "";

        public decimal PricePerNight { get; set; }

        public int Capacity { get; set; }

        public bool IsAvailable { get; set; }

        public int HotelId { get; set; }

        public Hotel? Hotel { get; set; }

        public ICollection<Reservation>? Reservations { get; set; }
    }
}