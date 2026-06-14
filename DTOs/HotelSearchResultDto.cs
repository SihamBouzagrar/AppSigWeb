namespace ReservationHotel.DTOs
{
    /// <summary>
    /// DTO retourné par GET api/hotels/search
    /// Contient uniquement les infos nécessaires pour afficher une carte hôtel dans les résultats
    /// </summary>
    public class HotelSearchResultDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string City { get; set; } = "";

        public string Description { get; set; } = "";

        /// <summary>Prix minimum parmi les chambres disponibles correspondant au nombre de guests</summary>
        public decimal MinPricePerNight { get; set; }

        public string MainImageUrl { get; set; } = "";

        public int AvailableRoomsCount { get; set; }
    }
}