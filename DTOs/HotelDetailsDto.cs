using ReservationHotel.Models;

namespace ReservationHotel.DTOs
{
    /// <summary>
    /// Détail complet d'un hôtel (page détail + GET api/hotels/{id})
    /// </summary>
    public class HotelDetailsDto
    {
        public int Id { get; set; }

        public string Nom { get; set; } = "";

        public string Description { get; set; } = "";

        public string Ville { get; set; } = "";
            public decimal   PrixParNuit{ get; set; }

        public List<string> Images { get; set; } = new();
public List<RoomDto> Rooms { get; set; } = new List<RoomDto>();
    
    }

    /// <summary>
    /// Représentation d'une chambre dans les réponses API et vues
    /// </summary>
}