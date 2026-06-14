using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationHotel.Services;
using ReservationHotel.Data;           // ← Important
using ReservationHotel.Models;
using ReservationHotel.DTOs;
namespace ReservationHotel.Controllers
{
    public class HotelsController : Controller
    {
        private readonly HotelService _hotelService;
        private readonly AppDbContext _context;   // ← Doit être AppDbContext, pas DbContext


        public HotelsController(AppDbContext context,HotelService hotelService)
        {
             _context = context;
            _hotelService = hotelService;
        }

public async Task<IActionResult> Index(string? search, string? city, DateTime? checkIn, DateTime? checkOut, int guests = 2)
{
    var query = _context.Hotels
        .Include(h => h.Images)
        .Include(h => h.Rooms)          // ← C'est ÇA qui manque probablement
        .AsQueryable();

    if (!string.IsNullOrWhiteSpace(search))
    {
        var term = search.ToLower();
        query = query.Where(h => h.Nom.ToLower().Contains(term) ||
                                h.Ville.ToLower().Contains(term) ||
                                h.Description.ToLower().Contains(term));
    }

    if (!string.IsNullOrWhiteSpace(city))
    {
        query = query.Where(h => h.Ville.ToLower().Contains(city.ToLower()));
    }

    var hotels = await query.ToListAsync();

    var hotelDtos = hotels.Select(h => new HotelDetailsDto
    {
        Id = h.Id,
        Nom = h.Nom ?? "",
        Ville = h.Ville ?? "",
        Description = h.Description ?? "",
        PrixParNuit = h.PrixParNuit,
        Images = h.Images?.Select(i => i.ImageUrl).ToList() ?? new List<string>(),
        Rooms = h.Rooms?.Select(r => new RoomDto
        {
            Id = r.Id,
            RoomType = r.RoomType ?? "",
            Capacity = r.Capacity,
            PricePerNight = r.PricePerNight,
            IsAvailable = r.IsAvailable
        }).ToList() ?? new List<RoomDto>()
    }).ToList();

    ViewBag.CheckIn = checkIn ?? DateTime.Today;
    ViewBag.CheckOut = checkOut ?? DateTime.Today.AddDays(3);
    ViewBag.Guests = guests;

    return View(hotelDtos);
}
   // GET /Hotels/Details/5
        // ✅ FIX: supprimé [HttpGet("{id}")] qui cassait le routing MVC conventionnel
        public async Task<IActionResult> Details(int id,
            DateTime? checkIn, DateTime? checkOut, int guests = 1)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);

            if (hotel == null)
                return NotFound();

            // ✅ FIX: on passe les paramètres de recherche au ViewBag
            // pour que la vue Details puisse les inclure dans les liens "Book Now"
            ViewBag.CheckIn = checkIn;
            ViewBag.CheckOut = checkOut;
            ViewBag.Guests = guests;

            return View(hotel);
        }
    }
}