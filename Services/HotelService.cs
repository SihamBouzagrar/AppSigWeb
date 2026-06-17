using Microsoft.EntityFrameworkCore;
using ReservationHotel.Data;
using ReservationHotel.DTOs;



namespace ReservationHotel.Services
{
    public class HotelService
    {
        private readonly AppDbContext _context;

        public HotelService(AppDbContext context)
        {
            _context = context;
        }

        // ── Recherche hôtels disponibles ──────────────────────────────
        public async Task<List<HotelSearchResultDto>> SearchHotelsAsync(
            string city, DateTime checkIn, DateTime checkOut, int guests)
        {
            return await _context.Hotels
                .AsNoTracking()
                .Where(h =>
                    h.Ville.ToLower().Contains(city.ToLower()) &&
                    // CS8604 FIX : les collections sont initialisées dans Hotel.cs → plus jamais null
                    h.Rooms.Any(r =>
                        r.Capacity >= guests &&
                        
                        !_context.Reservations.Any(res =>
                            res.RoomId == r.Id &&
                            checkIn < res.CheckOut &&
                            checkOut > res.CheckIn)))
                .Select(h => new HotelSearchResultDto
                {
                    Id = h.Id,
                    Name = h.Nom,
                    City = h.Ville,
                    Description = h.Description,
                    // CS8601 FIX : ?? "" garantit une string non-null
                    MainImageUrl = h.Images
    .Select(i => i.ImageUrl)
    .FirstOrDefault() ?? "/images/hotels/default.jpg",
                    // CS8604 FIX : (decimal?) cast évite le null sur Min() si aucune chambre
                    MinPricePerNight = h.Rooms
                        .Where(r => r.Capacity >= guests)
                        .Min(r => (decimal?)r.PricePerNight) ?? 0,
                    AvailableRoomsCount = h.Rooms.Count(r =>
                     
                        r.Capacity >= guests &&
                        !_context.Reservations.Any(res =>
                            res.RoomId == r.Id &&
                            checkIn < res.CheckOut &&
                            checkOut > res.CheckIn))
                })
                .ToListAsync();
        }

       // ── Détail d'un hôtel par ID ──────────────────────────────────
public async Task<HotelDetailsDto?> GetHotelByIdAsync(int id)
{
    return await _context.Hotels
        .AsNoTracking()
        .Where(h => h.Id == id)
        .Select(h => new HotelDetailsDto
        {
            Id = h.Id,
            Nom = h.Nom ?? "",
            Description = h.Description ?? "",
            Ville = h.Ville ?? "",
            Images = h.Images.Select(i => i.ImageUrl).ToList(),
            Rooms = h.Rooms.Select(r => new RoomDto
            {
                Id = r.Id,
                RoomType = r.RoomType ?? "",
                Capacity = r.Capacity,
                PricePerNight = r.PricePerNight,
               
            }).ToList()
        })
        .FirstOrDefaultAsync();
}

// ── Liste complète → page Hotels/Index ───────────────────────
public async Task<List<HotelDetailsDto>> GetAllHotelDtosAsync()
{
    return await _context.Hotels
        .AsNoTracking()
        .AsSplitQuery()
        .Select(h => new HotelDetailsDto
        {
            Id = h.Id,
            Nom = h.Nom ?? "",
            Description = h.Description ?? "",
            Ville = h.Ville ?? "",
            Images = h.Images.Select(i => i.ImageUrl).ToList(),
            Rooms = h.Rooms.Select(r => new RoomDto
            {
                Id = r.Id,
                RoomType = r.RoomType ?? "",
                Capacity = r.Capacity,
                PricePerNight = r.PricePerNight,
               
            }).ToList()
        })
        .ToListAsync();
}
    }
}