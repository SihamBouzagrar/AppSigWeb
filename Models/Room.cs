namespace ReservationHotel.Models
{
    public class Room
    {
        public int Id { get; set; }

        public string RoomType { get; set; } = "";

        public decimal PricePerNight { get; set; }

        public int Capacity { get; set; }

      
      public int AvailableCount { get; set; }

        public Hotel? Hotel { get; set; }

        public ICollection<Reservation>? Reservations { get; set; }
    }
}