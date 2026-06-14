namespace ReservationHotel.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }
  public int Guests { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public decimal TotalPrice { get; set; }

    //    public int CustomerId { get; set; }
      //  public Customer? Customer { get; set; }

        public int RoomId { get; set; }
        public Room? Room { get; set; }
    }
}