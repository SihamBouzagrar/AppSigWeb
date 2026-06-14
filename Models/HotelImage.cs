namespace ReservationHotel.Models
{
    public class HotelImage
    {
        public int Id { get; set; }

        public int HotelId { get; set; }

        // CS8618 FIX : initialisé à "" — EF remplira la valeur depuis la BDD
        public string ImageUrl { get; set; } = "";

        // CS8618 FIX : navigation property marquée nullable avec ?
        // EF Core remplit la référence via Include(), pas besoin de valeur par défaut
        public Hotel? Hotel { get; set; }
    }
}