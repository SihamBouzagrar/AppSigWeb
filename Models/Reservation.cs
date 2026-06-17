using HotelReservation.Models;

namespace ReservationHotel.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }
        public string UserId { get; set; } = null!;
        public int Guests { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ApplicationUser User { get; set; }

        public decimal TotalPrice { get; set; }

        public int RoomId { get; set; }
        public Room? Room { get; set; }
        public ReservationStatus Status { get; set; }
    }
}