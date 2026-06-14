using System;

namespace ReservationHotel.ViewModels
{
    public class ReservationCreateViewModel
    {
        public int RoomId { get; set; }

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }

        public int Guests { get; set; }
    }
}