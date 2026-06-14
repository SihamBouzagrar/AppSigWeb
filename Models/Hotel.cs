namespace ReservationHotel.Models
{
    public class Hotel
    {
        public int Id { get; set; }

        public string Nom { get; set; } = "";

        public string Ville { get; set; } = "";

        public string Description { get; set; } = "";

        public decimal PrixParNuit { get; set; }

        // CS8618 FIX : collections initialisées → jamais null, EF les peuple via Include()
        public List<HotelImage> Images { get; set; } = new();

        public List<Room> Rooms { get; set; } = new();
    }
}